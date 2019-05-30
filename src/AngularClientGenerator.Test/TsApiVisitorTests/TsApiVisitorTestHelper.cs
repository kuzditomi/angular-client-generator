using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.PartBuilders;
using AngularClientGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    public class TsApiVisitorTestHelper
    {
        private readonly GeneratorConfig basicTsConfig;

        public TsApiVisitorTestHelper(ClientType clientType)
        {
            this.basicTsConfig = new GeneratorConfig
            {
                ClientType = clientType,
                IndentType = IndentType.Tab
            };
        }

        public string VisitEmptyModule(GeneratorConfig config)
        {
            var moduleDescription = new ModuleDescriptionPart
            {
                Name = "example",
                ControllerDescriptionParts = Enumerable.Empty<ControllerDescriptionPart>()
            };

            return this.VisitModule(moduleDescription, config);
        }

        public string VisitEmptyModule()
        {
            return this.VisitEmptyModule(this.basicTsConfig);
        }

        public string VisitModule(ModuleDescriptionPart moduleDescription, GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = CreateVisitor(config, builder);

            apiVisitor.Visit(moduleDescription);
            return apiVisitor.GetContent();
        }

        public string VisitModule(ModuleDescriptionPart moduleDescription)
        {
            return VisitModule(moduleDescription, this.basicTsConfig);
        }

        public string VisitController(ControllerDescriptor controllerDescriptor)
        {
            var builder = new ClientBuilder(this.basicTsConfig);
            var apiVisitor = CreateVisitor(this.basicTsConfig, builder);

            apiVisitor.Visit(new ControllerDescriptionPart(controllerDescriptor));
            return apiVisitor.GetContent();
        }

        public string VisitControllerInModule(ControllerDescriptor controllerDescriptor)
        {
            return this.VisitControllerInModule(controllerDescriptor, this.basicTsConfig);
        }

        public string VisitControllerInModule(ControllerDescriptor controllerDescriptor, GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = CreateVisitor(config, builder);

            var module = new ModuleDescriptionPart
            {
                Name = "ExampleModule",
                ControllerDescriptionParts = new List<ControllerDescriptionPart>
                {
                    new ControllerDescriptionPart(controllerDescriptor)
                }
            };

            apiVisitor.Visit(module);
            return apiVisitor.GetContent();
        }

        public string VisitAction(ActionDescriptor actionDescriptor)
        {
            return this.VisitAction(actionDescriptor, this.basicTsConfig);
        }

        public string VisitAction(ActionDescriptor actionDescriptor, GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = CreateVisitor(config, builder);

            apiVisitor.Visit(new ActionDescriptionPart(actionDescriptor));
            return apiVisitor.GetContent();
        }

        public string VisitActionInModule(ActionDescriptor actionDescriptor)
        {
            return this.VisitControllerInModule(new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor> { actionDescriptor }
            });
        }

        public string VisitActionInModule(ActionDescriptor actionDescriptor, GeneratorConfig config)
        {
            return this.VisitControllerInModule(new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor> { actionDescriptor }
            }, config);
        }

        private IApiVisitor CreateVisitor(GeneratorConfig config, ClientBuilder builder)
        {
            if (config.ClientType == ClientType.Angular)
                return new AngularApiVisitor(config, builder);

            if (config.ClientType == ClientType.AngularJsTypeScript)
                return new AngularJsTypescriptApiVisitor(config, builder);

            throw new NotImplementedException("No implementation given for client type");
        }
    }
}
