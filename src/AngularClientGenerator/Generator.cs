using System.IO;
using System.Web.Http.Description;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator
{
    public class Generator
    {
        private DescriptionCollector DescriptionCollector;
        private IApiVisitor Visitor;

        public GeneratorConfig Config { get; set; }

        public Generator(IApiExplorer explorer)
        {
            this.Config = new GeneratorConfig();
            this.DescriptionCollector = new DescriptionCollector(explorer);
        }

        public void Generate()
        {
            var builder = new ClientBuilder(this.Config);

            switch (this.Config.Language)
            {
                case Language.TypeScript:
                    this.Visitor = new TsApiVisitor(this.Config, builder);
                    break;
                default:
                    this.Visitor = new TsApiVisitor(this.Config, builder);
                    break;
            }

            var controllerDescriptions = DescriptionCollector.GetControllerDescriptions();
            foreach (var controllerDescription in controllerDescriptions)
            {
                controllerDescription.ActionDescriptionParts = DescriptionCollector.GetActionDescriptionsForController(controllerDescription.Name);
            }

            var moduleDescription = new ModuleDescriptionPart
            {
                Name = this.Config.ModuleName,
                ControllerDescriptionParts = controllerDescriptions
            };

            moduleDescription.Accept(this.Visitor);

            var content = this.Visitor.GetContent();

            File.WriteAllText(this.Config.ExportPath, content);
        }
    }
}
