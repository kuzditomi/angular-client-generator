using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.PartBuilders;
using AngularClientGenerator.Extensions;

namespace AngularClientGenerator.Visitors
{
    public class AngularJSTypescriptApiVisitor : TypescriptApiVisitorBase
    {
        public AngularJSTypescriptApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            this.ClientBuilder.WriteLine("export namespace GeneratedClient {{");
            this.ClientBuilder.IncreaseIndent();

            this.WriteModuleDefinition(moduleDescription.Name);
            this.WriteUrlConstants();
            this.WriteUrlReplaceMethod();

            foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            {
                controllerDescriptionPart.Accept(this);
            }

            this.WriteTypes();
            this.WriteEnumService();

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.ClientBuilder.WriteLine("export class Api{0}Service {{", controllerDescription.Name);
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("static $inject = ['$http', '$q'];", controllerDescription.Name);
            this.ClientBuilder.WriteLine("constructor(private http: ng.IHttpService, private q: ng.IQService) {{ }}", controllerDescription.Name);
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

        protected override void GenerateMethodFor(ActionDescriptionPart actionDescription)
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
            this.ClientBuilder.WriteLine("public {0} = ({1}): ng.IPromise<{2}> => {{", actionDescription.Name, parametersWithTypes, GetNameSpaceAndNameForType(actionDescription.ReturnValueDescription.Type));

            // call config
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("return this.http<{0}>(this.{1}Config({2}))", GetNameSpaceAndNameForType(actionDescription.ReturnValueDescription.Type), actionDescription.Name, parameters);
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
            this.ClientBuilder.WriteLine("Message: (resp.data && resp.data.Message) || resp.statusText,");
            this.ClientBuilder.WriteLine("Data: resp.data,");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}});");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}});");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.DecreaseIndent();

            // method footer
            this.ClientBuilder.WriteLine("}}");
        }

        protected override void GenerateConfigFor(ActionDescriptionPart actionDescription)
        {
            var needsUrlReplace = actionDescription.UrlTemplate.Contains("{");
            var hasParameter = actionDescription.ParameterDescriptions.Any();
            var isPostOrPut = actionDescription.HttpMethod == HttpMethod.Post ||
                              actionDescription.HttpMethod == HttpMethod.Put;

            var deleteHasComplexParam = actionDescription.HttpMethod == HttpMethod.Delete && actionDescription.ParameterDescriptions.Any(parameterDescription => parameterDescription.IsComplex());

            var shouldUseData = isPostOrPut || deleteHasComplexParam;

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

                this.ClientBuilder.WriteLine("public {0}Config({1}): ng.IRequestConfig {{",
                    actionDescription.Name,
                    parameters);
            }
            else
            {
                this.ClientBuilder.WriteLine("public {0}Config(): ng.IRequestConfig {{",
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
                this.ClientBuilder.WriteLine("url: replaceUrl(API_BASE_URL + '{0}', {{", actionDescription.UrlTemplate);
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
                this.ClientBuilder.WriteLine("url: API_BASE_URL + '{0}',", actionDescription.UrlTemplate);
            }

            this.ClientBuilder.WriteLine("method: '{0}',", actionDescription.HttpMethod.ToString().ToUpper());

            if (hasParameter)
            {
                if (shouldUseData)
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

            if (deleteHasComplexParam)
            {
                this.ClientBuilder.WriteLine("headers: {{");
                this.ClientBuilder.IncreaseIndent();
                this.ClientBuilder.WriteLine("'Content-Type': 'application/json',");
                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}},");
            }

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}};");

            // method footer
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        private void WriteEnumServiceInterface()
        {
            var enumtypes = Types.Where(t => t.Value.IsEnum).Select(t => t.Value).ToList();
            var enumTypeArr = string.Join("|", enumtypes.Select(e => $"'{e.Name}'"));
            var enumTypeObj = string.Join("|", enumtypes.Select(e => $"'{e.Name}Obj'"));

            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine($"export type EnumArr = {enumTypeArr};");
            this.ClientBuilder.WriteLine($"export type EnumObj = {enumTypeObj};");
            this.ClientBuilder.WriteLine("export type EnumValue = {{ Name: string, Value: number, Title: string }};");
            this.ClientBuilder.WriteLine("export type EnumArrayServiceType = {{ [K in EnumArr]?: Array<EnumValue>; }};");
            this.ClientBuilder.WriteLine("export type EnumObjServiceType = {{ [K in EnumObj]?: Record<string, EnumValue>; }};");
            this.ClientBuilder.WriteLine("export interface IEnumService extends EnumArrayServiceType, EnumObjServiceType, GeneratedClient.EnumHelperService {{ }}");
            this.ClientBuilder.WriteLine();
        }

        private void WriteEnumService()
        {
            this.WriteEnumServiceInterface();

            this.ClientBuilder.WriteLine("export class EnumHelperService {{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("[index: string]: any;");
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

        private void WriteModuleDefinition(string moduleName)
        {
            this.ClientBuilder.WriteLine("export let Module = angular.module('{0}', []);", moduleName);
        }
    }
}
