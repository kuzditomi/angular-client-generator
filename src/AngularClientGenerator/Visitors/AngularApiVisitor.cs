using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.PartBuilders;
using System.Collections.Generic;

namespace AngularClientGenerator.Visitors
{
    public class AngularApiVisitor : TypescriptApiVisitorBase
    {
        private ICollection<string> serviceNames;

        public AngularApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
            this.serviceNames = new List<string>();
        }

        protected override string ConfigReturnType => "RequestOptions";
        protected override string ConfigReturnClosing => "}} as RequestOptions;";

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            this.WriteImports();
            this.WriteHelperTypes();
            this.WriteUrlConstants();
            this.WriteUrlReplaceMethod();

            this.ClientBuilder.WriteLine("export namespace GeneratedAngularClient {{");
            this.ClientBuilder.IncreaseIndent();

            foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            {
                controllerDescriptionPart.Accept(this);
            }

            this.WriteModuleDefinition(moduleDescription.Name);
            this.WriteTypes();

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.serviceNames.Add($"{controllerDescription.Name}ApiService");

            this.ClientBuilder.WriteLine("@Injectable({{");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("providedIn: 'root'");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}})");

            this.ClientBuilder.WriteLine("export class {0}ApiService {{", controllerDescription.Name);
            this.ClientBuilder.IncreaseIndent();

            this.ClientBuilder.WriteLine("apiUrl: string = API_BASE_URL;");
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("constructor(private httpClient: HttpClient) {{");
            this.ClientBuilder.WriteLine("}}");

            foreach (var actionDescriptionPart in controllerDescription.ActionDescriptionParts)
            {
                actionDescriptionPart.Accept(this);
            }

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        protected override void GenerateMethodFor(ActionDescriptionPart actionDescription, GeneratedMethodInfo generatedMethodInfo)
        {
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("public {0}({1}): Observable<{2}> {{", actionDescription.Name, generatedMethodInfo.ParametersWithType, GetNameSpaceAndNameForType(actionDescription.ReturnValueDescription.Type));
            this.ClientBuilder.IncreaseIndent();

            this.ClientBuilder.WriteLine("const config = this.{0}Config({1});", actionDescription.Name, generatedMethodInfo.Parameters);
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("return this.httpClient.request(config.method, config.url, config);");

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        private void WriteModuleDefinition(string moduleName)
        {
            // module header
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("@NgModule({{");
            this.ClientBuilder.IncreaseIndent();

            //services
            this.ClientBuilder.WriteLine("providers: [");
            this.ClientBuilder.IncreaseIndent();

            foreach (var serviceName in this.serviceNames)
            {
                this.ClientBuilder.WriteLine("{0},", serviceName);
            }

            // TODO: service list
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("],");

            // imports
            this.ClientBuilder.WriteLine("imports: [");
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("HttpClientModule,");
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("],");

            // class declaration
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}})");
            this.ClientBuilder.WriteLine("export class {0} {{", moduleName);
            this.ClientBuilder.WriteLine("}}");
            this.ClientBuilder.WriteLine();
        }

        private void WriteImports()
        {
            this.ClientBuilder.WriteLine("import {{ Injectable, NgModule }} from '@angular/core';");
            this.ClientBuilder.WriteLine("import {{ HttpClientModule, HttpClient, HttpErrorResponse }} from '@angular/common/http';");
            this.ClientBuilder.WriteLine("import {{ Observable, throwError }} from 'rxjs';");
            this.ClientBuilder.WriteLine("import {{ catchError }} from 'rxjs/operators';");
        }

        private void WriteHelperTypes()
        {
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("type RequestOptions = Parameters<HttpClient[\"request\"]>[\"2\"] & {{ method: string, url: string }};");
        }
    }
}
