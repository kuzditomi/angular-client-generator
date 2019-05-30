﻿using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using System.Collections.Generic;
using System.Linq;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    public abstract class TsApiVisitorTestBase
    {
        private readonly GeneratorConfig basicTsConfig;

        public TsApiVisitorTestBase(ClientType clientType)
        {
            this.basicTsConfig = new GeneratorConfig
            {
                ClientType = clientType,
                IndentType = IndentType.Tab
            };
        }

        protected string VisitEmptyModule(GeneratorConfig config)
        {
            var moduleDescription = new ModuleDescriptionPart
            {
                Name = "example",
                ControllerDescriptionParts = Enumerable.Empty<ControllerDescriptionPart>()
            };

            return this.VisitModule(moduleDescription, config);
        }

        protected string VisitEmptyModule()
        {
            return this.VisitEmptyModule(this.basicTsConfig);
        }

        protected string VisitModule(ModuleDescriptionPart moduleDescription, GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = CreateVisitor(config, builder);
            
            apiVisitor.Visit(moduleDescription);
            return apiVisitor.GetContent();
        }

        protected string VisitModule(ModuleDescriptionPart moduleDescription)
        {
            return VisitModule(moduleDescription, this.basicTsConfig);
        }

        protected string VisitTsController(ControllerDescriptor controllerDescriptor)
        {
            var builder = new ClientBuilder(this.basicTsConfig);
            var apiVisitor = CreateVisitor(this.basicTsConfig, builder);

            apiVisitor.Visit(new ControllerDescriptionPart(controllerDescriptor));
            return apiVisitor.GetContent();
        }

        protected string VisitTsControllerInModule(ControllerDescriptor controllerDescriptor)
        {
            return this.VisitTsControllerInModule(controllerDescriptor, this.basicTsConfig);
        }

        protected string VisitTsControllerInModule(ControllerDescriptor controllerDescriptor, GeneratorConfig config)
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

        protected string VisitTsAction(ActionDescriptor actionDescriptor)
        {
            return this.VisitTsAction(actionDescriptor, this.basicTsConfig);
        }

        protected string VisitTsAction(ActionDescriptor actionDescriptor, GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = CreateVisitor(config, builder);

            apiVisitor.Visit(new ActionDescriptionPart(actionDescriptor));
            return apiVisitor.GetContent();
        }

        protected string VisitTsActionInModule(ActionDescriptor actionDescriptor)
        {
            return this.VisitTsControllerInModule(new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor> { actionDescriptor }
            });
        }

        protected string VisitTsActionInModule(ActionDescriptor actionDescriptor, GeneratorConfig config)
        {
            return this.VisitTsControllerInModule(new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor> { actionDescriptor }
            }, config);
        }

        private ApiVisitor CreateVisitor(GeneratorConfig config, ClientBuilder builder)
        {
            return config.ClientType == ClientType.Angular ? new AngularApiVisitor(config, builder) : new AngularJSTypescriptApiVisitor(config, builder);
        }
    }
}
