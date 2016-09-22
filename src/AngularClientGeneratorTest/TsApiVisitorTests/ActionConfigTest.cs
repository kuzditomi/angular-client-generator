using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using AngularClientGenerator;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGeneratorTest.TestControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGeneratorTest.TsApiVisitorTests
{
    [TestClass]
    public class ActionConfigTest : TestBase
    {
        [TestMethod]
        public void VoidParameterlessActionConfig()
        {
            RegisterController<ConfigTestController>();

            RunInScope(() =>
            {
                var content = VisitModule();
                var expectedLines = new List<string>
                {
                    "public VoidAction() : ng.IPromise<void> {",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent));
            });
        }

        private string VisitModule()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = Language.TypeScript
            };
            var builder = new ClientBuilder(config);
            var apiVisitor = new TsApiVisitor(config, builder);

            var apiDescriptions = ApiExplorer
                .ApiDescriptions
                .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "ConfigTest")
                .Select(a => a.ActionDescriptor);

            foreach (var httpActionDescriptor in apiDescriptions)
            {
                var actionDescriptorPart = new ActionDescriptionPart(httpActionDescriptor);
                actionDescriptorPart.Accept(apiVisitor);
            }

            return apiVisitor.GetContent();
        }
    }

}
