using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using Microsoft.Win32.SafeHandles;

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
