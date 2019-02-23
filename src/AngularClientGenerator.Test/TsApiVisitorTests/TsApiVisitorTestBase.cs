using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using System.Collections.Generic;
using System.Linq;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    public class TsApiVisitorTestBase
    {
        private readonly GeneratorConfig basicTsConfig = new GeneratorConfig
        {
            IndentType = IndentType.Tab,
            Language = Language.TypeScript
        };

        protected string VisitEmptyModule(GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);
            var moduleDescription = new ModuleDescriptionPart
            {
                Name = "example",
                ControllerDescriptionParts = Enumerable.Empty<ControllerDescriptionPart>()
            };

            apiVisitor.Visit(moduleDescription);
            return apiVisitor.GetContent();
        }

        protected string VisitEmptyTsModule()
        {
            return this.VisitEmptyModule(this.basicTsConfig);
        }

        protected string VisitTsController(ControllerDescriptor controllerDescriptor)
        {
            var builder = new ClientBuilder(this.basicTsConfig);
            var apiVisitor = new AngularJSTypescriptApiVisitor(this.basicTsConfig, builder);

            apiVisitor.Visit(new ControllerDescriptionPart(controllerDescriptor));
            return apiVisitor.GetContent();
        }

        protected string VisitTsAction(ActionDescriptor actionDescriptor)
        {
            return this.VisitTsAction(actionDescriptor, this.basicTsConfig);
        }

        protected string VisitTsAction(ActionDescriptor actionDescriptor, GeneratorConfig config)
        {
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

            apiVisitor.Visit(new ActionDescriptionPart(actionDescriptor));
            return apiVisitor.GetContent();
        }
    }
}
