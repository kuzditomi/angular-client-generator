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
            return new List<string>
            {
                "public ArrayReturnAction(): Observable<IMyEmptyTestClass[]> {",
                "\tconst config = this.ArrayReturnActionConfig();",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidComplexparamAction()
        {
            return new List<string>
            {
                "public VoidComplexparamAction(complex: IMyEmptyTestClass): Observable<void> {",
                "\tconst config = this.VoidComplexparamActionConfig(complex);",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
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
            return new List<string>
            {
                "public VoidStringParamGetAction(stringparameter: string): Observable<void> {",
                "\tconst config = this.VoidStringParamGetActionConfig(stringparameter);",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
        }

        protected override IEnumerable<string> ExpectedGenericTypeReturnAction(string @namespace)
        {
            return new List<string>
            {
                $"public GenericTypeReturnAction(): Observable<{@namespace}.IGenericTypeClass<number>> {{",
                "\tconst config = this.GenericTypeReturnActionConfig();",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
        }

        protected override IEnumerable<string> ExpectedResponseTypeAttributeReturnAction()
        {
            return new List<string>
            {
                "public ResponseTypeReturnAction(): Observable<IMyEmptyTestClass> {",
                "\tconst config = this.ResponseTypeReturnActionConfig();",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
        }

        protected override IEnumerable<string> ExpectedStringReturnAction()
        {
            return new List<string>
            {
                "public StringReturnAction(): Observable<string> {",
                "\tconst config = this.StringReturnActionConfig();",
                "",
                "\treturn this.httpClient.request(config.method, config.url, config);",
                "}"
            };
        }
    }
}