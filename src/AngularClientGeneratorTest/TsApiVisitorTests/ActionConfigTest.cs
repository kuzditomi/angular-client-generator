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
                    "public VoidParameterlessGetActionConfig() : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/void',",
                    "\t\tmethod: 'GET'",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);
                
                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void VoidStringParameterActionConfig()
        {
            RegisterController<ConfigTestController>();

            RunInScope(() =>
            {
                var content = VisitModule();
                var expectedLines = new List<string>
                {
                    "public VoidStringParameterActionConfig(stringparam: string) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidstring',",
                    "\t\tmethod: 'GET'",
                    "\t\tparams: {",
                    "\t\t\tstringparam: stringparam",
                    "\t\t}",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void VoidIntParameterActionConfig()
        {
            RegisterController<ConfigTestController>();

            RunInScope(() =>
            {
                var content = VisitModule();
                var expectedLines = new List<string>
                {
                    "public VoidIntParameterActionConfig(intparam: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidint',",
                    "\t\tmethod: 'GET'",
                    "\t\tparams: {",
                    "\t\t\tintparam: intparam",
                    "\t\t}",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void VoidDoubleParameterActionConfig()
        {
            RegisterController<ConfigTestController>();

            RunInScope(() =>
            {
                var content = VisitModule();
                var expectedLines = new List<string>
                {
                    "public VoidDoubleParameterActionConfig(doubleparam: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voiddouble',",
                    "\t\tmethod: 'GET'",
                    "\t\tparams: {",
                    "\t\t\tdoubleparam: doubleparam",
                    "\t\t}",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void VoidDecimalParameterActionConfig()
        {
            RegisterController<ConfigTestController>();

            RunInScope(() =>
            {
                var content = VisitModule();
                var expectedLines = new List<string>
                {
                    "public VoidDecimalParameterActionConfig(decimalparam: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voiddecimal',",
                    "\t\tmethod: 'GET'",
                    "\t\tparams: {",
                    "\t\t\tdecimalparam: decimalparam",
                    "\t\t}",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void VoidSimpleParametersActionConfig()
        {
            RegisterController<ConfigTestController>();

            RunInScope(() =>
            {
                var content = VisitModule();
                var expectedLines = new List<string>
                {
                    "public VoidSimpleParametersActionConfig(a: string, b: number, c: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidsimpleparams',",
                    "\t\tmethod: 'GET'",
                    "\t\tparams: {",
                    "\t\t\ta: a",
                    "\t\t\tb: b",
                    "\t\t\tc: c",
                    "\t\t}",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "ConfigTest");

            foreach (var httpActionDescriptor in apiDescriptions)
            {
                var actionDescriptorPart = new ActionDescriptionPart(httpActionDescriptor);
                actionDescriptorPart.Accept(apiVisitor);
            }

            return apiVisitor.GetContent();
        }
    }

}
