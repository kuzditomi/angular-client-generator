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
        protected TsApiVisitorTestHelper visitor;

        public TsApiVisitorTestsBase(ClientType clientType)
        {
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
    }
}
