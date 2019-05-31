using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularApiVisitorTests
{
    [TestClass]
    public class AngularGeneratedActionTest : GeneratedActionTestBase
    {
        public AngularGeneratedActionTest() : base(ClientType.Angular)
        {
        }

        [TestMethod]
        public override void GeneratedVoidParameterlessActionTest()
        {
            // Arrange
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidParameterlessGetAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            // Act
            var content = this.visitor.VisitAction(actionDescriptor);

            // Assert
            var expectedLines = new List<string>
            {
                "public VoidParameterlessGetAction(): Observable<void> {",
                "\tconst config = this.VoidParameterlessGetActionConfig();",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        public override void ArrayReturnActionTest(Type arrayType)
        {
            throw new NotImplementedException();
        }

        public override void GeneratedVoidComplexparamActionTest()
        {
            throw new NotImplementedException();
        }

        public override void GeneratedVoidStringparamActionTest()
        {
            throw new NotImplementedException();
        }

        public override void GenericTypeReturnActionTest()
        {
            throw new NotImplementedException();
        }

        public override void ResponseTypeAttributeReturnActionTest()
        {
            throw new NotImplementedException();
        }

        public override void StringReturnActionTest()
        {
            throw new NotImplementedException();
        }
    }
}