using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class AngularJsTypescriptGeneratedActionTest : GeneratedActionTestBase
    {
        public AngularJsTypescriptGeneratedActionTest() : base(ClientType.AngularJsTypeScript)
        {
        }

        private readonly IEnumerable<string> httpThenPart = new List<string> {
            "\t\t.then(resp => {",
            "\t\t\treturn resp.data;",
            "\t\t}, resp => {",
            "\t\t\treturn this.q.reject({",
            "\t\t\t\tStatus: resp.status,",
            "\t\t\t\tMessage: (resp.data && resp.data.Message) || resp.statusText,",
            "\t\t\t\tData: resp.data,",
            "\t\t\t});",
            "\t\t});"
        };

        [TestMethod]
        public void HttpThenPart()
        {
            var content = this.visitor.VisitAction(new ActionDescriptor
            {
                Name = "a",
                HttpMethod = HttpMethod.Delete,
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor
                {
                    Type = typeof(string)
                },
                UrlTemplate = "a"
            });
            var expectedLines = httpThenPart;
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidParameterlessAction()
        {
            return new List<string>
            {
                "public VoidParameterlessGetAction = (): ng.IPromise<void> => {",
                "\treturn this.http<void>(this.VoidParameterlessGetActionConfig())",
            }.Concat(httpThenPart);
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidStringparamAction()
        {
            return new List<string>
            {
                "public VoidStringParamGetAction = (stringparameter: string): ng.IPromise<void> => {",
                "\treturn this.http<void>(this.VoidStringParamGetActionConfig(stringparameter))",
            }.Concat(httpThenPart);
        }

        protected override IEnumerable<string> ExpectedGeneratedVoidComplexparamAction()
        {
            return new List<string>
            {
                "public VoidComplexparamAction = (complex: IMyEmptyTestClass): ng.IPromise<void> => {",
                "\treturn this.http<void>(this.VoidComplexparamActionConfig(complex))",
            }.Concat(httpThenPart);
        }

        protected override IEnumerable<string> ExpectedStringReturnAction()
        {
            return new List<string>
                {
                    "public StringReturnAction = (): ng.IPromise<string> => {",
                    "\treturn this.http<string>(this.StringReturnActionConfig())",
                }.Concat(httpThenPart);
        }

        protected override IEnumerable<string> ExpectedResponseTypeAttributeReturnAction()
        {
            return new List<string>
                {
                    "public ResponseTypeReturnAction = (): ng.IPromise<IMyEmptyTestClass> => {",
                    "\treturn this.http<IMyEmptyTestClass>(this.ResponseTypeReturnActionConfig())",
                }.Concat(httpThenPart);
        }

        protected override IEnumerable<string> ExpectedArrayReturnAction()
        {
            return new List<string>
                {
                    "public ArrayReturnAction = (): ng.IPromise<IMyEmptyTestClass[]> => {",
                    "\treturn this.http<IMyEmptyTestClass[]>(this.ArrayReturnActionConfig())",
                }.Concat(httpThenPart);
        }

        protected override IEnumerable<string> ExpectedGenericTypeReturnAction(string @namespace)
        {
            return new List<string>
                {
                    $"public GenericTypeReturnAction = (): ng.IPromise<{@namespace}.IGenericTypeClass<number>> => {{",
                    $"\treturn this.http<{@namespace}.IGenericTypeClass<number>>(this.GenericTypeReturnActionConfig())",
                }.Concat(httpThenPart);
        }
    }
}