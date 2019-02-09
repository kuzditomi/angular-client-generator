using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Config;
using AngularClientGeneratorTest.TestControllers;

namespace AngularClientGeneratorTest.TsApiVisitorTests
{
    [TestClass]
    public class GeneratedActionTest: TestBase
    {
        private IEnumerable<string> httpThenPart;

        public GeneratedActionTest()
        {
            httpThenPart = new List<string>
            {
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
        }

        [TestMethod]
        public void HttpThenPart()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = httpThenPart;
                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });

        }

        [TestMethod]
        public void GeneratedVoidParameterlessActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidParameterlessGetAction = (): ng.IPromise<void> => {",
                    "\treturn this.http<void>(this.VoidParameterlessGetActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void GeneratedVoidStringparamActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidStringParamGetAction = (stringparameter: string): ng.IPromise<void> => {",
                    "\treturn this.http<void>(this.VoidStringParamGetActionConfig(stringparameter))",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void GeneratedVoidComplexparamActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidComplexparamAction = (complex: IMyEmptyTestClass): ng.IPromise<void> => {",
                    "\treturn this.http<void>(this.VoidComplexparamActionConfig(complex))",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void StringReturnActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public StringReturnAction = (): ng.IPromise<string> => {",
                    "\treturn this.http<string>(this.StringReturnActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void ResponseTypeAttributeReturnActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public ResponseTypeReturnAction = (): ng.IPromise<IMyEmptyTestClass> => {",
                    "\treturn this.http<IMyEmptyTestClass>(this.ResponseTypeReturnActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void ArrayReturnActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public ArrayReturnAction = (): ng.IPromise<IMyEmptyTestClass[]> => {",
                    "\treturn this.http<IMyEmptyTestClass[]>(this.ArrayReturnActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void IEnumerableReturnActionTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public IEnumerableReturnAction = (): ng.IPromise<IMyEmptyTestClass[]> => {",
                    "\treturn this.http<IMyEmptyTestClass[]>(this.IEnumerableReturnActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void GenericTypeReturnActionTest()
        {
            RegisterController<GeneratedMethodTestController>();
            var _namespace = "Test";

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    UseNamespaces = true,
                    NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGeneratorTest.TestModels", _namespace)
                };
                var content = VisitActionsFromController<GeneratedMethodTestController>(config);
                var expectedLines = new List<string>
                {
                    $"public GenericTypeReturnAction = (): ng.IPromise<{_namespace}.IGenericTypeClass<number>> => {{",
                    $"\treturn this.http<{_namespace}.IGenericTypeClass<number>>(this.GenericTypeReturnActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = string.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), string.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }
    }
}
