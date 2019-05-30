using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace AngularClientGenerator.Visitor
{
    public abstract class TypescriptApiVisitor : ApiVisitor
    {
        protected readonly List<KeyValuePair<string, Type>> Types = new List<KeyValuePair<string, Type>>();

        protected TypescriptApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
        }

        protected void WriteTypes()
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

        protected void WriteUrlConstants()
        {
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("let addr = window['ApiHost'];");
            this.ClientBuilder.WriteLine("if (addr.indexOf('ApiHost') !== -1) {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine($"addr = '{Config.DefaultBaseUrl}';");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("export const BASE_URL = addr;");
            this.ClientBuilder.WriteLine($"export const API_SUFFIX = '{Config.UrlSuffix}';");
            this.ClientBuilder.WriteLine("export const API_BASE_URL = BASE_URL + API_SUFFIX;");
            this.ClientBuilder.WriteLine();
        }

        protected void WriteUrlReplaceMethod()
        {
            this.ClientBuilder.WriteLine("function replaceUrl(url: string, params: any) {{");
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

        protected string GetNameForType(Type type)
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

            if (type.Name == "IHttpActionResult" || type == typeof(HttpResponseMessage))
            {
                return "any";
            }

            if (type == typeof(DateTime))
            {
                return "string";
            }

            if (type.IsGenericParameter)
            {
                // Suitable only for one generic argument
                return "T";
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
                else if (keyType == typeof(string))
                {
                    keyTypeName = "string";
                }
                else
                {
                    throw new ArgumentException("Dictionary can have only numbers or strings as a key");
                }

                string valueTypeName = GetNameSpaceAndNameForType(valueType);

                return $"{{[key: {keyTypeName}]: {valueTypeName}}}";
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

            if (typeDescPart.IsGeneric())
            {
                // Suitable only for one generic argument
                var name = type.GetGenericTypeDefinition().Name;
                return "I" + name.Split('`')[0] + "<T>";
            }

            return "I" + type.Name;
        }

        protected string GetNameSpaceAndNameForType(Type type)
        {
            // dont use namespace for system types
            var name = GetNameForType(type);
            if (!this.Config.UseNamespaces)
                return name;

            if (!type.IsEnum && !name.StartsWith("I"))
                return name;

            if (name.EndsWith("[]"))
                return name;

            if (type.IsGenericType)
            {
                // Suitable only for one generic argument
                name = name.Replace("<T>", $"<{GetNameSpaceAndNameForType(type.GetGenericArguments()[0])}>");
            }

            var @namespace = this.Config.NamespaceNamingRule(type);
            return $"{@namespace}.{name}";
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
                var name = GetNameForType(type);
                this.ClientBuilder.WriteLine("export interface {0} {{", name);
                this.ClientBuilder.IncreaseIndent();

                var typeToCheckPropertiesIn = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                foreach (var propertyInfo in typeToCheckPropertiesIn.GetProperties())
                {
                    var isPropertyNullable = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    var nullablePrefix = isPropertyNullable ? "?" : string.Empty;
                    var typename = GetNameSpaceAndNameForType(propertyInfo.PropertyType);
                    this.ClientBuilder.WriteLine("{0}{1}: {2};", propertyInfo.Name, nullablePrefix, typename);
                }

                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}}");
                this.ClientBuilder.WriteLine();
            }
        }
    }
}
