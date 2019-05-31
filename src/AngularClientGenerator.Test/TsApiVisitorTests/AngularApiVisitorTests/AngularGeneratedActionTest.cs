using System.Collections.Generic;
using AngularClientGenerator.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularApiVisitorTests
{
    [TestClass]
    public class AngularGeneratedActionTest : GeneratedActionTestBase
    {
        public AngularGeneratedActionTest() : base(ClientType.Angular)
        {
        }

        protected override IEnumerable<string> ExpectedArrayReturnAction()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidComplexparamAction()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidParameterlessAction()
        {
            return new List<string>
            {
                "public VoidParameterlessGetAction(): Observable<void> {",
                "\tconst config = this.VoidParameterlessGetActionConfig();",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidStringparamAction()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<string> ExpectedGenericTypeReturnAction(string @namespace)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<string> ExpectedResponseTypeAttributeReturnAction()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<string> ExpectedStringReturnAction()
        {
            throw new System.NotImplementedException();
        }
    }
}