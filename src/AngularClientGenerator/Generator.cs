using System;
using System.IO;
using System.Web.Http.Description;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Exceptions;
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
            ValidateConfig();
            var builder = new ClientBuilder(this.Config);

            if (this.Config.Language == Language.TypeScript)
            {
                this.Visitor = new AngularJSTypescriptApiVisitor(this.Config, builder);
            }
            else
            {
                throw new NotSupportedException("Requested language is not supported: " + this.Config.Language);
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

        private void ValidateConfig()
        {
            if (!string.IsNullOrEmpty(this.Config.UrlSuffix) && !this.Config.UrlSuffix.EndsWith("/"))
            {
                throw new GeneratorConfigurationException("Url suffix must end with slash");
            }

            if (!string.IsNullOrEmpty(this.Config.DefaultBaseUrl) && !this.Config.DefaultBaseUrl.EndsWith("/"))
            {
                throw new GeneratorConfigurationException("DefaultBaseUrl must end with slash");
            }
        }
    }

}
