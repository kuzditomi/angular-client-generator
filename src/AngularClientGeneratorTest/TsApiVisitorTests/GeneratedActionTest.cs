using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGeneratorTest.TestControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGeneratorTest.TsApiVisitorTests
{
    [TestClass]
    public class GeneratedActionTest : TestBase
    {
        private IEnumerable<string> httpThenPart;

        public GeneratedActionTest()
        {
            httpThenPart = new List<string>
            {
                "\t\t.then(function(resp) {",
                "\t\t\treturn resp.data;",
                "\t\t});",
                "}",
            };
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
                    "public VoidParameterlessGetAction() : ng.IPromise<void> {",
                    "\treturn this.http(this.VoidParameterlessGetActionConfig())",
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
                    "public VoidStringParamGetAction(stringparameter: string) : ng.IPromise<void> {",
                    "\treturn this.http(this.VoidStringParamGetActionConfig(stringparameter))",
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
                    "public VoidComplexparamAction(complex: IMyEmptyTestClass) : ng.IPromise<void> {",
                    "\treturn this.http(this.VoidComplexparamActionConfig(complex))",
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
                    "public StringReturnAction() : ng.IPromise<string> {",
                    "\treturn this.http(this.StringReturnActionConfig())",
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
                    "public ResponseTypeReturnAction() : ng.IPromise<IMyEmptyTestClass> {",
                    "\treturn this.http(this.ResponseTypeReturnActionConfig())",
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
                    "public ArrayReturnAction() : ng.IPromise<IMyEmptyTestClass[]> {",
                    "\treturn this.http(this.ArrayReturnActionConfig())",
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
                    "public IEnumerableReturnAction() : ng.IPromise<IMyEmptyTestClass[]> {",
                    "\treturn this.http(this.IEnumerableReturnActionConfig())",
                }.Concat(httpThenPart);

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }
        
    }
}
