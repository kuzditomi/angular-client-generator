using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class ActionConfigTest : TestBaseWithHelper
    {
        public ActionConfigTest() : base(ClientType.AngularJsTypeScript)
        {
        }

        [TestMethod]
        public void VoidParameterlessActionConfig()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidParameterlessGetAction",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/void"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidParameterlessGetActionConfig(): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/void',",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [DataTestMethod]
        [DataRow(typeof(int), "number")]
        [DataRow(typeof(double), "number")]
        [DataRow(typeof(decimal), "number")]
        [DataRow(typeof(float), "number")]
        [DataRow(typeof(string), "string")]
        [DataRow(typeof(Guid), "string")]
        [DataRow(typeof(DateTime), "string")]
        [DataRow(typeof(DateTime), "string")]
        public void QueryParameterTypeActionConfig(Type paramType, string generatedParamType)
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "QueryParamTestAction",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "typetestparam",
                        ParameterType = paramType
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/typetest"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    $"public QueryParamTestActionConfig(typetestparam: {generatedParamType}): ng.IRequestConfig {{",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/typetest',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\ttypetestparam: typetestparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void VoidSimpleParametersActionConfig()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidSimpleParametersAction",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "a",
                        ParameterType = typeof(string)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "b",
                        ParameterType = typeof(int)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "c",
                        ParameterType = typeof(double)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/voidsimpleparams"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidSimpleParametersActionConfig(a: string, b: number, c: number): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidsimpleparams',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\ta: a,",
                    "\t\t\tb: b,",
                    "\t\t\tc: c,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [DataTestMethod]
        [DataRow("GET")]
        [DataRow("POST")]
        [DataRow("DELETE")]
        public void VoidReplaceNumberParametersActionConfig(string httpMethod)
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidReplaceNumberAction",
                HttpMethod = new HttpMethod(httpMethod),
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "myparameter",
                        ParameterType = typeof(int)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/{myparameter}"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidReplaceNumberActionConfig(myparameter: number): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/configtest/{myparameter}', {",
                    "\t\t\tmyparameter: myparameter,",
                    "\t\t}),",
                    $"\t\tmethod: '{httpMethod}',",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void VoidReplaceMoreParamsActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidReplaceMoreParamsAction",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "id",
                        ParameterType = typeof(int)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "second",
                        ParameterType = typeof(string)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/{id}/more/{second}"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);

            var expectedLines = new List<string>
                {
                    "public VoidReplaceMoreParamsActionConfig(id: number, second: string): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/configtest/{id}/more/{second}', {",
                    "\t\t\tid: id,",
                    "\t\t\tsecond: second,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [DataTestMethod]
        [DataRow("POST")]
        [DataRow("PUT")]
        [DataRow("DELETE")]
        public void VoidComplexParamActionTest(string httpMethod)
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidComplexParamPostAction",
                HttpMethod = new HttpMethod(httpMethod),
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "complex",
                        ParameterType = typeof(MyEmptyTestClass)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/voidcomplexparampost"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidComplexParamPostActionConfig(complex: IMyEmptyTestClass): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidcomplexparampost',",
                    $"\t\tmethod: '{httpMethod}',",
                    "\t\tdata: complex,"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void VoidComplexParamAndReplaceActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidComplexParamAndReplaceAction",
                HttpMethod = HttpMethod.Put,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "id",
                        ParameterType = typeof(string)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "complex",
                        ParameterType = typeof(MyEmptyTestClass)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/voidcomplexparamandreplace/{id}"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidComplexParamAndReplaceActionConfig(id: string, complex: IMyEmptyTestClass): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/configtest/voidcomplexparamandreplace/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'PUT',",
                    "\t\tdata: complex,",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void VoidComplexParamAndReplaceGetActionTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "VoidComplexParamAndReplaceGetAction",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "id",
                        ParameterType = typeof(string)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "complex",
                        ParameterType = typeof(MyEmptyTestClass)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/voidcomplexparamandreplaceget/{id}"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public VoidComplexParamAndReplaceGetActionConfig(id: string, complex: IMyEmptyTestClass): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/configtest/voidcomplexparamandreplaceget/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: complex,",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ComplexParamAndReplaceGetActionErrorTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "formaterror",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "id",
                        ParameterType = typeof(string)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "complex",
                        ParameterType = typeof(MyEmptyTestClass)
                    },
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "a",
                        ParameterType = typeof(string)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/voidcomplexparamandreplacegeterror/{id}"
            };

            this.visitor.VisitTsAction(actionDescriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BigintParamErrorTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "biginterror",
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "random",
                        ParameterType = typeof(long)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/configtest/biginterror"
            };

            this.visitor.VisitTsAction(actionDescriptor);
        }

        [TestMethod]
        public void DeleteMethodBodyTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "DeleteComplexParam",
                HttpMethod = HttpMethod.Delete,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "param",
                        ParameterType = typeof(ComplexDeleteType)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/methodtest/deletedata"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "\t\theaders: {",
                    "\t\t\t'Content-Type': 'application/json',",
                    "\t\t},",
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void ParamsPropertyForDeleteMethodUrlTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "DeleteSimpleParam",
                HttpMethod = HttpMethod.Delete,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "param",
                        ParameterType = typeof(string)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/methodtest/deletesimpledata"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public DeleteSimpleParamConfig(param: string): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/methodtest/deletesimpledata',",
                    "\t\tmethod: 'DELETE',",
                    "\t\tparams: {",
                    "\t\t\tparam: param,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void EnumerableParamsPropertyForDeleteMethodUrlTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "DeleteSimpleEnumerableParam",
                HttpMethod = HttpMethod.Delete,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "param",
                        ParameterType = typeof(int[])
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/methodtest/deletesimpleenumerabledata"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public DeleteSimpleEnumerableParamConfig(param: number[]): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/methodtest/deletesimpleenumerabledata',",
                    "\t\tmethod: 'DELETE',",
                    "\t\tparams: {",
                    "\t\t\tparam: param,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }

        [TestMethod]
        public void DeleteUrlReplaceAndBody()
        {

            var actionDescriptor = new ActionDescriptor
            {
                Name = "DeleteUrlReplaceAndBody",
                HttpMethod = HttpMethod.Delete,
                ParameterDescriptors = new List<ParameterDescriptor> {
                    new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "id",
                        ParameterType = typeof(int)
                    },
                     new ParameterDescriptor
                    {
                        IsOptional = false,
                        ParameterName = "param",
                        ParameterType = typeof(ComplexDeleteType)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) },
                UrlTemplate = "api/methodtest/deleteurlreplaceandbody/{id}"
            };

            var content = this.visitor.VisitTsAction(actionDescriptor);
            var expectedLines = new List<string>
                {
                    "public DeleteUrlReplaceAndBodyConfig(id: number, param: IComplexDeleteType): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/methodtest/deleteurlreplaceandbody/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'DELETE',",
                    "\t\tdata: param,",
                    "\t\theaders: {",
                    "\t\t\t'Content-Type': 'application/json',",
                    "\t\t},",
                    "\t};",
                    "}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
        }
    }
}
