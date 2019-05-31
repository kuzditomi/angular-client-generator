using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    [TestClass]
    public abstract class GeneratedActionTestBase : TestBaseWithHelper
    {
        public GeneratedActionTestBase(ClientType clientType) : base(clientType)
        {
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

            var content = this.visitor.VisitAction(actionDescriptor);
            var expectedLines = ExpectedGeneratedVoidParameterlessAction();

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedGeneratedVoidParameterlessAction();

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

            var content = this.visitor.VisitAction(actionDescriptor);
            var expectedLines = ExpectedGeneratedVoidStringparamAction();

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedGeneratedVoidStringparamAction();

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

            var content = this.visitor.VisitAction(actionDescriptor);
            var expectedLines = ExpectedGeneratedVoidComplexparamAction();
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedGeneratedVoidComplexparamAction();

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

            var content = this.visitor.VisitAction(actionDescriptor);
            var expectedLines = ExpectedStringReturnAction();
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedStringReturnAction();

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

            var content = this.visitor.VisitAction(actionDescriptor);
            var expectedLines = ExpectedResponseTypeAttributeReturnAction();
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedResponseTypeAttributeReturnAction();

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

            var content = this.visitor.VisitAction(actionDescriptor);
            var expectedLines = ExpectedArrayReturnAction();
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedArrayReturnAction();

        [TestMethod]
        public void GenericTypeReturnActionTest()
        {
            var @namespace = "Test";

            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.TestModels", @namespace),
                ClientType = this.clientType
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "GenericTypeReturnAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(GenericTypeClass<int>) }
            };

            var content = this.visitor.VisitAction(actionDescriptor, config);
            var expectedLines = ExpectedGenericTypeReturnAction(@namespace);
            var expectedContent = string.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), string.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
        }
        protected abstract IEnumerable<string> ExpectedGenericTypeReturnAction(string @namespace);
    }
}