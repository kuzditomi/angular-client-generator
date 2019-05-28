using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public class AngularApiVisitor : AngularJSTypescriptApiVisitor
    {
        public AngularApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            this.WriteUrlConstants();
            this.WriteUrlReplaceMethod();

            //foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            //{
            //    controllerDescriptionPart.Accept(this);
            //}

            this.WriteModuleDefinition(moduleDescription.Name);

            this.ClientBuilder.WriteLine("export namespace GeneratedAngularClient {{");
            this.ClientBuilder.IncreaseIndent();

            this.WriteTypes();

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            throw new System.NotImplementedException();
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
