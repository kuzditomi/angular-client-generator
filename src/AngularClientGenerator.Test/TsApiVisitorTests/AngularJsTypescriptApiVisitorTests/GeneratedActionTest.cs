using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class GeneratedActionTest : TsApiVisitorTestsBase
    {
        public GeneratedActionTest() : base(ClientType.AngularJsTypeScript)
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
            var content = this.visitor.VisitTsAction(new ActionDescriptor
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

        [TestMethod]
        public void GeneratedVoidParameterlessActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidParameterlessGetAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidParameterlessGetAction = (): ng.IPromise<void> => {",
                    "\treturn this.http<void>(this.VoidParameterlessGetActionConfig())",
                }.Concat(httpThenPart);

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void GeneratedVoidStringparamActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidStringParamGetAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        ParameterName = "stringparameter",
                        ParameterType = typeof(string),
                        IsOptional = false
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
            {
                "public VoidStringParamGetAction = (stringparameter: string): ng.IPromise<void> => {",
                "\treturn this.http<void>(this.VoidStringParamGetActionConfig(stringparameter))",
            }.Concat(httpThenPart);

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void GeneratedVoidComplexparamActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidComplexparamAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        ParameterName = "complex",
                        ParameterType = typeof(MyEmptyTestClass),
                        IsOptional = false
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
            {
                "public VoidComplexparamAction = (complex: IMyEmptyTestClass): ng.IPromise<void> => {",
                "\treturn this.http<void>(this.VoidComplexparamActionConfig(complex))",
            }.Concat(httpThenPart);

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void StringReturnActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "StringReturnAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(string) }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public StringReturnAction = (): ng.IPromise<string> => {",
                    "\treturn this.http<string>(this.StringReturnActionConfig())",
                }.Concat(httpThenPart);

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void ResponseTypeAttributeReturnActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "ResponseTypeReturnAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(MyEmptyTestClass) }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public ResponseTypeReturnAction = (): ng.IPromise<IMyEmptyTestClass> => {",
                    "\treturn this.http<IMyEmptyTestClass>(this.ResponseTypeReturnActionConfig())",
                }.Concat(httpThenPart);

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [DataTestMethod]
        [DataRow(typeof(IEnumerable<MyEmptyTestClass>))]
        [DataRow(typeof(List<MyEmptyTestClass>))]
        [DataRow(typeof(MyEmptyTestClass[]))]
        public void ArrayReturnActionTest(Type arrayType)
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "ArrayReturnAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = arrayType }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public ArrayReturnAction = (): ng.IPromise<IMyEmptyTestClass[]> => {",
                    "\treturn this.http<IMyEmptyTestClass[]>(this.ArrayReturnActionConfig())",
                }.Concat(httpThenPart);

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void GenericTypeReturnActionTest()
        {
            var _namespace = "Test";

            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.TestModels", _namespace)
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "GenericTypeReturnAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(GenericTypeClass<int>) }
            };

            var content = this.visitor.VisitTsAction(actionDescriptor, config);
            var expectedLines = new List<string>
                {
                    $"public GenericTypeReturnAction = (): ng.IPromise<{_namespace}.IGenericTypeClass<number>> => {{",
                    $"\treturn this.http<{_namespace}.IGenericTypeClass<number>>(this.GenericTypeReturnActionConfig())",
                }.Concat(httpThenPart);

            var expectedContent = string.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), string.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
    }
}