using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGenerator.Contracts.Descriptors;
using System.Net.Http;
using AngularClientGenerator.Test.TestModels;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    [TestClass]
    public class TsApiVisitorTest : TsApiVisitorTestBase
    {
        [TestMethod]
        public void ControllerDescriptionPart_TestController()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = Language.TypeScript
            };
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);
            var controllerDesciptionPart = new ControllerDescriptionPart(new ControllerDescriptor
            {
                Name = "Test",
                ActionDescriptors = new List<ActionDescriptor>()
            });

            apiVisitor.Visit(controllerDesciptionPart);

            var content = apiVisitor.GetContent();

            var expectedContents = new List<string> {
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
        }

        [TestMethod]
        public void ActionDescriptionPart_TestController()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = Language.TypeScript
            };
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

            var actionDescriptors = new string[] { "A", "B", "Random" }.Select(name => new ActionDescriptor
            {
                Name = name,
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(string) },
                UrlTemplate = string.Empty
            });

            foreach (var actionDescriptor in actionDescriptors)
            {
                var actionDescriptorPart = new ActionDescriptionPart(actionDescriptor);
                actionDescriptorPart.Accept(apiVisitor);
            }

            var content = apiVisitor.GetContent();

            var expectedMethods = new string[] { "A", "B", "Random" };
            var expectedContents = new List<string>();

            foreach (var methodName in expectedMethods)
            {
                expectedContents.Add(String.Format("public {0}", methodName));
            }

            foreach (var expectedContent in expectedContents)
            {
                Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            }
        }

        [TestMethod]
        public void TypeDescriptionPart_TestController()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = Language.TypeScript
            };
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "Test",
                ActionDescriptors = new List<ActionDescriptor>
                {
                    new ActionDescriptor
                    {
                        Name = "A",
                        HttpMethod = HttpMethod.Get,
                        ParameterDescriptors = new List<ParameterDescriptor> {
                            new ParameterDescriptor {
                                ParameterType = typeof(TestModelA)
                            }
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(TestModelB) },
                        UrlTemplate = string.Empty
                    }
                }
            };

            var moduleDescription = new ModuleDescriptionPart
            {
                ControllerDescriptionParts = new List<ControllerDescriptionPart> {
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
        }

        [TestMethod]
        public void ApiHostIsInitialisedFromWindow()
        {
            var actualContent = VisitEmptyTsModule();
            var expectedContent = "\tlet addr = window['ApiHost'];";

            Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
        }


        [TestMethod]
        public void DefaultBaseURLFromConfig()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = Language.TypeScript,
                DefaultBaseUrl = "myexampleurl"
            };

            var actualContent = VisitEmptyModule(config);
            var expectedContent = "\t\taddr = 'myexampleurl';";

            Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
        }

        [TestMethod]
        public void UrlSuffixAddition()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = Language.TypeScript,
                DefaultBaseUrl = "mybaseurl",
                UrlSuffix = "abc"
            };

            var actualContent = VisitEmptyModule(config);
            var expectedContents = new List<string>
                {
                    "\t\taddr = 'mybaseurl';",
                    "\texport const API_SUFFIX = 'abc';",
                };

            foreach (var expectedContent in expectedContents)
            {
                Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            }
        }
    }
}
