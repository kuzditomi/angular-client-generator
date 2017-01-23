using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public class TsApiVisitor : ApiVisitor
    {
        private List<KeyValuePair<string, Type>> Types { get; }

        public TsApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
            this.Types = new List<KeyValuePair<string, Type>>();
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.ClientBuilder.WriteLine("export class Api{0}Service {{", controllerDescription.Name);
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("static $inject = ['$http', '$q'];", controllerDescription.Name);
            this.ClientBuilder.WriteLine("constructor(private http, private q){{ }}", controllerDescription.Name);
            this.ClientBuilder.WriteLine();

            foreach (var actionDescriptionPart in controllerDescription.ActionDescriptionParts)
            {
                actionDescriptionPart.Accept(this);
            }

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}", controllerDescription.Name);
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("Module.service('Api{0}Service', Api{0}Service);", controllerDescription.Name);
            this.ClientBuilder.WriteLine();
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
            if (this.Config.UseNamespaces && this.Config.NamespaceNamingRule == null)
                throw new ArgumentNullException("Config.NamespaceNamingRule");

            // visit return value type
            actionDescription.ReturnValueDescription.Accept(this);

            // visit parameter types
            foreach (var actionDescriptionParameterDescription in actionDescription.ParameterDescriptions)
            {
                actionDescriptionParameterDescription.Accept(this);
            }

            GenerateConfigFor(actionDescription);
            GenerateMethodFor(actionDescription);
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            this.ClientBuilder.WriteLine("export namespace GeneratedClient {{");
            this.ClientBuilder.IncreaseIndent();

            this.ClientBuilder.WriteLine("export let Module = angular.module('{0}', []);", moduleDescription.Name);
            this.ClientBuilder.WriteLine();

            this.GenerateUrlReplaceMethod();

            foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            {
                controllerDescriptionPart.Accept(this);
            }

            this.WriteTypes();
            this.WriteEnumService();

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }


        public override void Visit(TypeDescriptionPart typeDescriptionPart)
        {
            if (typeDescriptionPart.Type == typeof(Int64))
                throw new ArgumentException("Int64(long) is not supported in typescript.", nameof(typeDescriptionPart));

            var generatedName = GetNameForType(typeDescriptionPart.Type);
            var sameName = this.Types.Where(t => t.Key == generatedName).ToList();
            if (sameName.Count > 0)
            {
                if (sameName.Any(t => t.Value.FullName == typeDescriptionPart.Type.FullName))
                    return;

                if (!this.Config.UseNamespaces)
                    throw new FormatException("There are two types with the same name but different fullname. Please use namespaces!");

                if (this.Config.NamespaceNamingRule == null)
                    throw new ArgumentNullException("Config.NamespaceNamingRule");
            }

            if (typeDescriptionPart.IsIgnoredType())
                return;

            if (typeDescriptionPart.IsDictionary())
            {
                Type valueType = typeDescriptionPart.Type.GetGenericArguments()[1];
                this.Visit(new TypeDescriptionPart(valueType));
                return;
            }

            if (typeDescriptionPart.Type.IsArray)
            {
                var elementType = typeDescriptionPart.Type.GetElementType();
                this.Visit(new TypeDescriptionPart(elementType));
                return;
            }

            if (typeDescriptionPart.IsTask())
            {
                return;
            }

            if (typeDescriptionPart.IsEnumerable() || typeDescriptionPart.IsNullable())
            {
                var genericType = typeDescriptionPart.Type.GetGenericArguments()[0];
                this.Visit(new TypeDescriptionPart(genericType));
                return;
            }

            this.Types.Add(new KeyValuePair<string, Type>(generatedName, typeDescriptionPart.Type));

            // visit properties
            foreach (var propertyInfo in typeDescriptionPart.Type.GetProperties())
            {
                this.Visit(new TypeDescriptionPart(propertyInfo.PropertyType));
            }
        }

        private void GenerateMethodFor(ActionDescriptionPart actionDescription)
        {
            var parametersWithTypes = String.Join(", ",
                actionDescription.ParameterDescriptions.Select(
                    p =>
                    {
                        var optionalPrefix = p.IsOptional ? "?" : string.Empty;
                        return $"{p.ParameterName}{optionalPrefix}: {GetNameSpaceAndNameForType(p.Type)}";
                    }));

            var parameters = String.Join(", ",
                actionDescription.ParameterDescriptions.Select(
                    p => p.ParameterName));

            // method header
            this.ClientBuilder.WriteLine("public {0} = ({1}) : ng.IPromise<{2}> => {{", actionDescription.Name, parametersWithTypes, GetNameSpaceAndNameForType(actionDescription.ReturnValueDescription.Type));

            // call config
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("return this.http(this.{0}Config({1}))", actionDescription.Name, parameters);
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine(".then(resp => {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("return resp.data;");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}, resp => {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("return this.q.reject({{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("Status: resp.status,");
            this.ClientBuilder.WriteLine("Message: (resp.data && resp.data.Message) || resp.statusText");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}});");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}});");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.DecreaseIndent();

            // method footer
            this.ClientBuilder.WriteLine("}}");
        }

        private void GenerateConfigFor(ActionDescriptionPart actionDescription)
        {
            var needsUrlReplace = actionDescription.UrlTemplate.Contains("{");
            var hasParameter = actionDescription.ParameterDescriptions.Any();
            var isPostOrPut = actionDescription.HttpMethod == HttpMethod.Post ||
                              actionDescription.HttpMethod == HttpMethod.Put;

            var paramsToReplace = actionDescription.ParameterDescriptions
                .Where(a => actionDescription.UrlTemplate.Contains("{" + a.ParameterName + "}"))
                .ToList();
            var paramsToNotReplace = actionDescription.ParameterDescriptions
                .Where(a => !actionDescription.UrlTemplate.Contains("{" + a.ParameterName + "}"))
                .ToList();

            // method header
            if (hasParameter)
            {
                var parameters = string.Join(", ", actionDescription.ParameterDescriptions.Select(p =>
                {
                    var optionalPrefix = p.IsOptional ? "?" : string.Empty;

                    return $"{p.ParameterName}{optionalPrefix}: {GetNameSpaceAndNameForType(p.Type)}";
                }));

                this.ClientBuilder.WriteLine("public {0}Config({1}) : ng.IRequestConfig {{",
                    actionDescription.Name,
                    parameters);
            }
            else
            {
                this.ClientBuilder.WriteLine("public {0}Config() : ng.IRequestConfig {{",
                    actionDescription.Name);
            }
            this.ClientBuilder.IncreaseIndent();

            // method body
            this.ClientBuilder.WriteLine("return {{");
            this.ClientBuilder.IncreaseIndent();

            if (needsUrlReplace && !hasParameter)
                throw new InvalidOperationException($"Needs to replace in url, but no parameters given:{actionDescription.Name}");

            if (needsUrlReplace)
            {
                this.ClientBuilder.WriteLine("url: replaceUrl('{0}', {{", actionDescription.UrlTemplate);
                this.ClientBuilder.IncreaseIndent();

                foreach (var actionDescriptionParameterDescription in paramsToReplace)
                {
                    this.ClientBuilder.WriteLine("{0}: {0},", actionDescriptionParameterDescription.ParameterName);
                }

                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}}),", actionDescription.UrlTemplate);
            }
            else
            {
                this.ClientBuilder.WriteLine("url: '{0}',", actionDescription.UrlTemplate);
            }

            this.ClientBuilder.WriteLine("method: '{0}',", actionDescription.HttpMethod.ToString().ToUpper());

            if (hasParameter)
            {
                if (isPostOrPut)
                {
                    if (paramsToNotReplace.Count > 1)
                        throw new ArgumentException($"Error with {actionDescription.UrlTemplate} : More complex type to add in POST/PUT request, please wrap them.");

                    if (paramsToNotReplace.Count == 1)
                        this.ClientBuilder.WriteLine("data: {0},", paramsToNotReplace.First().ParameterName);
                }
                else if (paramsToNotReplace.Any())
                {
                    if (paramsToNotReplace.Any(p => p.IsComplex()))
                    {
                        if (paramsToNotReplace.Count > 1)
                            throw new FormatException("Action can have only 1 complex type as param which is not replaced or more simple.");

                        this.ClientBuilder.WriteLine("params: {0},", paramsToNotReplace.First().ParameterName);
                    }
                    else
                    {
                        this.ClientBuilder.WriteLine("params: {{");
                        this.ClientBuilder.IncreaseIndent();
                        foreach (var actionDescriptionParameterDescription in paramsToNotReplace)
                        {
                            this.ClientBuilder.WriteLine("{0}: {0},", actionDescriptionParameterDescription.ParameterName);
                        }
                        this.ClientBuilder.DecreaseIndent();
                        this.ClientBuilder.WriteLine("}},");
                    }
                }
            }

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}};");

            // method footer
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        private void GenerateUrlReplaceMethod()
        {
            this.ClientBuilder.WriteLine("function replaceUrl(url: string, params: {{ }}) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("let replaced = url;");
            this.ClientBuilder.WriteLine("for (let key in params) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("if (params.hasOwnProperty(key)) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("const param = params[key];");
            this.ClientBuilder.WriteLine("replaced = replaced.replace('{{' + key + '}}', param);");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
            this.ClientBuilder.WriteLine("return replaced;");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
            this.ClientBuilder.WriteLine();
        }

        private void WriteEnumService()
        {
            this.ClientBuilder.WriteLine("export class EnumHelperService {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("constructor() {{");
            this.ClientBuilder.IncreaseIndent();

            var enumtypes = Types.Where(t => t.Value.IsEnum).Select(t => t.Value);
            foreach (var enumtype in enumtypes)
            {
                this.ClientBuilder.WriteLine("this.Register('{0}', {1}, {{", enumtype.Name, GetNameSpaceAndNameForType(enumtype));
                this.ClientBuilder.IncreaseIndent();

                var names = Enum.GetNames(enumtype);
                var memberList = names.Select(name =>
                {
                    var value = Enum.Parse(enumtype, name);
                    FieldInfo fi = enumtype.GetField(value.ToString());
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    var title = attributes.Length == 0 ? value.ToString() : attributes[0].Description;

                    return new { Title = title, Name = name };
                }).ToList();

                foreach (var enummember in memberList)
                {
                    this.ClientBuilder.WriteLine("{0}: '{1}',", enummember.Name, enummember.Title);
                }

                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}});", enumtype.Name, GetNameSpaceAndNameForType(enumtype));
            }

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.DecreaseIndent();
            this.WriteStaticPart("EnumHelper.ts.template");
        }

        private void WriteTypes()
        {
            if (this.Config.UseNamespaces)
            {
                var namespaceGroups = this.Types.GroupBy(t => t.Value.Namespace);

                foreach (var namespaceGroup in namespaceGroups)
                {
                    var namespaceName = this.Config.NamespaceNamingRule(namespaceGroup.First().Value);
                    this.ClientBuilder.WriteLine("export namespace {0} {{", namespaceName);
                    this.ClientBuilder.IncreaseIndent();

                    foreach (var type in namespaceGroup)
                    {
                        this.WriteType(type.Value);
                    }

                    this.ClientBuilder.DecreaseIndent();
                    this.ClientBuilder.WriteLine("}}");
                }
            }
            else
            {
                foreach (var type in Types)
                {
                    this.WriteType(type.Value);
                }
            }
        }

        private void WriteType(Type type)
        {
            if (type.IsEnum)
            {
                this.ClientBuilder.WriteLine("export enum {0} {{", type.Name);
                this.ClientBuilder.IncreaseIndent();

                foreach (var enumvalue in type.GetEnumValues())
                {
                    this.ClientBuilder.WriteLine("{0} = {1},", enumvalue.ToString(), (int)enumvalue);
                }

                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}}");
            }
            else
            {
                this.ClientBuilder.WriteLine("export interface {0} {{", GetNameForType(type));
                this.ClientBuilder.IncreaseIndent();

                foreach (var propertyInfo in type.GetProperties())
                {
                    var isPropertyNullable = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    var nullablePrefix = isPropertyNullable ? "?" : string.Empty;
                    this.ClientBuilder.WriteLine("{0}{1}: {2};", propertyInfo.Name, nullablePrefix, GetNameSpaceAndNameForType(propertyInfo.PropertyType));
                }

                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}}");
                this.ClientBuilder.WriteLine();
            }
        }

        private string GetNameSpaceAndNameForType(Type type)
        {
            // dont use namespace for system types
            var name = GetNameForType(type);
            if (!this.Config.UseNamespaces)
                return name;

            if (!type.IsEnum && !name.StartsWith("I"))
                return name;

            if (name.EndsWith("[]"))
                return name;

            var @namespace = this.Config.NamespaceNamingRule(type);
            return $"{@namespace}.{name}";
        }

        private string GetNameForType(Type type)
        {
            var typeDescPart = new TypeDescriptionPart(type);

            if (type == typeof(void))
            {
                return "void";
            }

            if (type.IsEnum)
            {
                return type.Name;
            }

            if (type == typeof(string) || type == typeof(Guid))
            {
                return "string";
            }

            if (type == typeof(bool))
            {
                return "boolean";
            }

            if (typeDescPart.IsNumberType())
            {
                return "number";
            }

            if (type == typeof(IHttpActionResult) || type == typeof(HttpResponseMessage))
            {
                return "any";
            }

            if (type == typeof(DateTime))
            {
                return "string";
            }

            if (typeDescPart.IsDictionary())
            {
                Type keyType = type.GetGenericArguments()[0];
                Type valueType = type.GetGenericArguments()[1];

                string keyTypeName;

                var keyTypeDesc = new TypeDescriptionPart(keyType);
                if (keyTypeDesc.IsNumberType())
                {
                    keyTypeName = "number";
                }
                else if(keyType == typeof(string) )
                {
                    keyTypeName = "string";
                }
                else
                {
                    throw new ArgumentException("Dictionary can have only numbers or strings as a key");
                }

                string valueTypeName = GetNameSpaceAndNameForType(valueType);

                return $"{{[key: {keyTypeName}]:{valueTypeName}}}";
            }

            if (typeDescPart.IsTask())
            {
                if (type.IsGenericType)
                {
                    return GetNameSpaceAndNameForType(type.GetGenericArguments()[0]);
                }
                else
                {
                    return "void";
                }
            }

            if (typeDescPart.IsEnumerable())
            {
                var isArray = type.IsArray;
                if (isArray)
                {
                    var typeofArray = type.GetElementType();
                    return GetNameSpaceAndNameForType(typeofArray) + "[]";
                }
                else
                {
                    var genericType = type.GetGenericArguments()[0];
                    return GetNameSpaceAndNameForType(genericType) + "[]";
                }
            }

            if (typeDescPart.IsNullable())
            {
                var genericType = type.GetGenericArguments()[0];
                return GetNameSpaceAndNameForType(genericType);
            }

            return "I" + type.Name;
        }
    }
}
