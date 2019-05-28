using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;
using System.Collections.Generic;

namespace AngularClientGenerator.Visitor
{
    public class AngularApiVisitor : AngularJSTypescriptApiVisitor
    {
        private ICollection<string> serviceNames;

        public AngularApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
            this.serviceNames = new List<string>();
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            this.WriteUrlConstants();
            this.WriteUrlReplaceMethod();

            foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            {
                controllerDescriptionPart.Accept(this);
            }

            this.WriteModuleDefinition(moduleDescription.Name);

            this.ClientBuilder.WriteLine("export namespace GeneratedAngularClient {{");
            this.ClientBuilder.IncreaseIndent();

            this.WriteTypes();

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.serviceNames.Add($"{controllerDescription.Name}ApiService");

            this.ClientBuilder.WriteLine("@Injectable()");
            this.ClientBuilder.WriteLine("export class {0}ApiService {{", controllerDescription.Name);
            this.ClientBuilder.IncreaseIndent();

            this.ClientBuilder.WriteLine("apiUrl: string = API_BASE_URL;");
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("constructor(private http: HttpClient) {{");
            this.ClientBuilder.WriteLine("}}");

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
            throw new System.NotImplementedException();
        }

        public override void Visit(TypeDescriptionPart typeDescriptionPart)
        {
            throw new System.NotImplementedException();
        }

        private void WriteModuleDefinition(string moduleName)
        {
            // module header
            this.ClientBuilder.WriteLine("@NgModule({{");
            this.ClientBuilder.IncreaseIndent();

            //services
            this.ClientBuilder.WriteLine("declarations: [");
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
        }
    }
}
