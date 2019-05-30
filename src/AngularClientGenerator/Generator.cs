using System;
using System.IO;
using System.Linq;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Contracts.Exceptions;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator
{
    public class Generator
    {
        private ApiDescriptor ApiDescriptor;
        private IApiVisitor Visitor;

        public GeneratorConfig Config { get; set; }

        public Generator(ApiDescriptor apiDescriptor)
        {
            this.Config = new GeneratorConfig();
            this.ApiDescriptor = apiDescriptor;
        }

        public void Generate()
        {
            ValidateConfig();
            var builder = new ClientBuilder(this.Config);

            if (this.Config.ClientType == ClientType.AngularJsTypeScript)
            {
                this.Visitor = new AngularJSTypescriptApiVisitor(this.Config, builder);
            }
            else
            {
                throw new NotSupportedException("Requested language is not supported: " + this.Config.ClientType);
            }

            var controllerDescriptions = ApiDescriptor.ControllerDescriptors
                .Select(cd => new ControllerDescriptionPart(cd));

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
