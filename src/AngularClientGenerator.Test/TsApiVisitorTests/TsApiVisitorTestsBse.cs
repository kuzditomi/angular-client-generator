using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    public abstract class TsApiVisitorTestsBase
    {
        private ClientType clientType;
        protected TsApiVisitorTestHelper visitor;

        protected TsApiVisitorTestsBase(ClientType clientType)
        {
            this.clientType = clientType;
            this.visitor = new TsApiVisitorTestHelper(clientType);
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
    }
}
