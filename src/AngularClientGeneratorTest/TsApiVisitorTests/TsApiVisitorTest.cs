using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGeneratorTest.TestControllers;
using AngularClientGenerator.Descriptor;

namespace AngularClientGeneratorTest.TsApiVisitorTests
{
    [TestClass]
    public class TsApiVisitorTest : TestBase
    {
        [TestMethod]
        public void ControllerDescriptionPart_TestController()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
                var builder = new ClientBuilder(config);
                var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

                var descriptor = ApiDescriptorConverter.CreateApiDescriptor(ApiExplorer);
                var controllerDescriptor = descriptor.ControllerDescriptors.Single(c => c.Name == "Test");
                var controllerDesciptionPart = new ControllerDescriptionPart(controllerDescriptor);

                apiVisitor.Visit(controllerDesciptionPart);

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>
                {
                    "export class ApiTestService {",
                    "}",
                    ".service('ApiTestService', ApiTestService)",
                    "static $inject = ['$http', '$q']",
                    "constructor(private http: ng.IHttpService, private q: ng.IQService)"
                };

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }

        [TestMethod]
        public void ActionDescriptionPart_TestController()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
                var builder = new ClientBuilder(config);
                var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

                var descriptor = ApiDescriptorConverter.CreateApiDescriptor(ApiExplorer);
                var controllerDescriptor = descriptor.ControllerDescriptors.Single(c => c.Name == "Test");

                var apiDescriptions = ApiExplorer
                    .ApiDescriptions
                    .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "Test");

                foreach (var actionDescriptor in controllerDescriptor.ActionDescriptors)
                {
                    var actionDescriptorPart = new ActionDescriptionPart(actionDescriptor);
                    actionDescriptorPart.Accept(apiVisitor);
                }

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>();

                foreach (var apiDescription in apiDescriptions)
                {
                    var methodName = apiDescription.ActionDescriptor.ActionName;
                    expectedContents.Add(String.Format("public {0}", methodName));
                }

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }

        [TestMethod]
        public void TypeDescriptionPart_TestController()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
                var builder = new ClientBuilder(config);
                var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);
                var descriptor = ApiDescriptorConverter.CreateApiDescriptor(ApiExplorer);

                var controllerDescriptor = descriptor.ControllerDescriptors.First(c => c.Name == "Test");

                var moduleDescription = new ModuleDescriptionPart
                {
                    ControllerDescriptionParts = new List<ControllerDescriptionPart>
                    {
                        new ControllerDescriptionPart(controllerDescriptor)
                    }
                };

                moduleDescription.Accept(apiVisitor);

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>
                {
                    "export interface ITestModelA {",
                    "export interface ITestModelB {",
                };

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }

        [TestMethod]
        public void ApiHostIsInitialisedFromWindow()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript,
                    DefaultBaseUrl = "http://localhost:1337"
                };

                var content = VisitModuleWithController<TestController>(config);

                var expectedContent = "\tlet addr = window.ApiHost;";

                Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            });
        }

        [TestMethod]
        public void DefaultBaseURLFromConfig()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript,
                    DefaultBaseUrl = "myexampleurl"
                };

                var content = VisitModuleWithController<TestController>(config);

                var expectedContent = "\t\taddr = 'myexampleurl';";

                Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            });
        }


        [TestMethod]
        public void UrlSuffixAddition()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript,
                    DefaultBaseUrl = "mybaseurl",
                    UrlSuffix = "abc"
                };

                var content = VisitModuleWithController<TestController>(config);

                var expectedContents = new List<string>
                {
                    "\t\taddr = 'mybaseurl';",
                    "\texport const API_SUFFIX = 'abc';",
                };

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }
    }
}
