using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public class TsApiVisitor : ApiVisitor
    {
        private HashSet<Type> Types { get; }

        public TsApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
            this.Types = new HashSet<Type>();
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
            this.ClientBuilder.WriteLine("GeneratedClient.factory('Api{0}Service', Api{0}Service);", controllerDescription.Name);
            this.ClientBuilder.WriteLine();
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
            // collect return value type for later
            actionDescription.ReturnValueDescription.Accept(this);

            // collect parameter types for later
            foreach (var actionDescriptionParameterDescription in actionDescription.ParameterDescriptions)
            {
                actionDescriptionParameterDescription.Accept(this);
            }

            GenerateConfigFor(actionDescription);
            GenerateMethodFor(actionDescription);
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            this.ClientBuilder.WriteLine("import {{module}} from 'angular';");
            this.ClientBuilder.WriteLine();

            this.ClientBuilder.WriteLine("export namespace GeneratedClient {{");
            this.ClientBuilder.IncreaseIndent();

            this.ClientBuilder.WriteLine("export let GeneratedClient = module('{0}', []);", moduleDescription.Name);
            this.ClientBuilder.WriteLine();

            this.GenerateUrlReplaceMethod();

            foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            {
                controllerDescriptionPart.Accept(this);
            }

            this.WriteTypes();

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        private static readonly Type[] ignoredTypesOnDefinition = { typeof(int), typeof(double), typeof(float), typeof(decimal), typeof(string), typeof(bool), typeof(void) };

        public override void Visit(TypeDescriptionPart typeDescriptionPart)
        {
            if (ignoredTypesOnDefinition.Contains(typeDescriptionPart.Type))
                return;

            var isArray = typeDescriptionPart.Type.GetInterfaces().Any(ti => ti == typeof(IEnumerable));
            if (isArray)
                return;

            this.Types.Add(typeDescriptionPart.Type);
        }

        private void GenerateMethodFor(ActionDescriptionPart actionDescription)
        {
            var parametersWithTypes = String.Join(", ",
                actionDescription.ParameterDescriptions.Select(
                    p => String.Format("{0}: {1}", p.ParameterName, GetNameForType(p.Type))));

            var parameters = String.Join(", ",
                actionDescription.ParameterDescriptions.Select(
                    p => p.ParameterName));

            // method header
            this.ClientBuilder.WriteLine("public {0}({1}) : ng.IPromise<{2}> {{", actionDescription.Name, parametersWithTypes, GetNameForType(actionDescription.ReturnValueDescription.Type));

            // call config
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("return this.http(this.{0}Config({1}))", actionDescription.Name, parameters);
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine(".then(function(resp) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("return resp.data;");
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
                    return String.Format("{0}: {1}", p.ParameterName, GetNameForType(p.Type));
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
                throw new InvalidOperationException(String.Format("Needs to replace in url, but no parameters given:{0}", actionDescription.Name));

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
                        throw new ArgumentException(String.Format("Error with {0} : More complex type to add in POST/PUT request, please wrap them.", actionDescription.UrlTemplate));

                    if (paramsToNotReplace.Count == 1)
                        this.ClientBuilder.WriteLine("data: {0},", paramsToNotReplace.First().ParameterName);
                }
                else if (paramsToNotReplace.Any())
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
            this.ClientBuilder.WriteLine("var replaced = url;");
            this.ClientBuilder.WriteLine("for (var key in params) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("if (params.hasOwnProperty(key)) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("var param = params[key];");
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

        private void WriteTypes()
        {
            foreach (var type in Types)
            {
                this.WriteType(type);
            }
        }

        private void WriteType(Type type)
        {
            if (type.IsEnum)
            {
                this.ClientBuilder.WriteLine("export enum {0} {{", type.Name);
                this.ClientBuilder.IncreaseIndent();

                foreach (var enumName in type.GetEnumNames())
                {
                    this.ClientBuilder.WriteLine("{0},", enumName);
                }

                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}}");
            }
            else
            {
                this.ClientBuilder.WriteLine("export interface {0} {{ }}", GetNameForType(type));
            }
        }

        private static readonly Type[] numberTypes = { typeof(int), typeof(double), typeof(float), typeof(decimal) };

        private string GetNameForType(Type type)
        {
            if (type == typeof(void))
            {
                return "void";
            }

            if (type.IsEnum)
            {
                return type.Name;
            }

            if (type == typeof(string))
            {
                return "string";
            }

            if (type == typeof(bool))
            {
                return "boolean";
            }

            if (numberTypes.Contains(type))
            {
                return "number";
            }

            var isIEnumerable = type.GetInterfaces().Any(ti => ti == typeof(IEnumerable));
            if (isIEnumerable)
            {
                var isArray = type.IsArray;
                if (isArray)
                {
                    var typeofArray = type.GetElementType();
                    return GetNameForType(typeofArray) + "[]";
                }
                else
                {
                    var genericType = type.GetGenericArguments()[0];
                    return GetNameForType(genericType) + "[]";
                }
            }

            return "I" + type.Name;
        }
    }
}
