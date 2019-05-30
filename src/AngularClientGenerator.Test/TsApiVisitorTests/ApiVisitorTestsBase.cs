using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    public abstract class ApiVisitorTestsBase : TestBaseWithHelper
    {
        protected ApiVisitorTestsBase(ClientType clientType) : base(clientType)
        {
        }

        [TestMethod]
        public void TypeDescriptionPartTest()
        {
            var actionDescriptor = new ActionDescriptor
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
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

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
            var actualContent = this.visitor.VisitEmptyModule();
            var expectedContent = "let addr = window['ApiHost'];";

            Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
        }

        [TestMethod]
        public void DefaultBaseURLFromConfig()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                ClientType = this.clientType,
                DefaultBaseUrl = "myexampleurl"
            };

            var actualContent = this.visitor.VisitEmptyModule(config);
            var expectedContent = "\taddr = 'myexampleurl';";

            Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
        }

        [TestMethod]
        public void UrlSuffixAddition()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                ClientType = this.clientType,
                DefaultBaseUrl = "mybaseurl",
                UrlSuffix = "abc"
            };

            var actualContent = this.visitor.VisitEmptyModule(config);
            var expectedContents = new List<string>
                {
                    "\taddr = 'mybaseurl';",
                    "export const API_SUFFIX = 'abc';",
                };

            foreach (var expectedContent in expectedContents)
            {
                Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            }
        }

        [TestMethod]
        public void ActionDescriptionPartTest()
        {
            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "testController",
                ActionDescriptors = new string[] { "A", "B", "Random" }.Select(name => new ActionDescriptor
                {
                    Name = name,
                    HttpMethod = HttpMethod.Get,
                    ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                    ReturnValueDescriptor = new TypeDescriptor { Type = typeof(string) },
                    UrlTemplate = string.Empty
                })
            };

            var actualContent = this.visitor.VisitTsControllerInModule(controllerDescriptor);
            var expectedContents = new string[] { "A", "B", "Random" }.Select(name => $"public {name}");

            foreach (var expectedContent in expectedContents)
            {
                Assert.IsTrue(actualContent.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            }
        }
    }
}
