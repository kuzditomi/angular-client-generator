using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGeneratorTest.TestControllers;
using System;
using System.Collections.Generic;

namespace AngularClientGeneratorTest.TsApiVisitorTests
{
    [TestClass]
    public class ActionConfigTest : TestBase
    {
        [TestMethod]
        public void VoidParameterlessActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
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
            });
        }

        [TestMethod]
        public void VoidStringParameterActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidStringParameterActionConfig(stringparam: string): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidstring',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tstringparam: stringparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidIntParameterActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidIntParameterActionConfig(intparam: number): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidint',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tintparam: intparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidDoubleParameterActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidDoubleParameterActionConfig(doubleparam: number): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voiddouble',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tdoubleparam: doubleparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidDecimalParameterActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidDecimalParameterActionConfig(decimalparam: number): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voiddecimal',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tdecimalparam: decimalparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidGuidToIntParameterActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidGuidParameterActionConfig(guidparam: string): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidguid',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tguidparam: guidparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
                Assert.IsFalse(content.Contains("public interface IGuid[]"), "Guid type should not be defined as new type");
            });
        }

        [TestMethod]
        public void VoidSimpleParametersActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
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
            });
        }

        [TestMethod]
        public void VoidReplaceNumberParametersActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidReplaceNumberActionConfig(id: number): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/configtest/voidreplaceparams/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidReplaceMoreParamsActionTest()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidReplaceMoreParamsActionConfig(id: number, second: string): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl(API_BASE_URL + 'api/configtest/voidreplacemoreparams/{id}/more/{second}', {",
                    "\t\t\tid: id,",
                    "\t\t\tsecond: second,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidComplexParamPostActionTest()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidComplexParamPostActionConfig(complex: IMyEmptyTestClass): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidcomplexparampost',",
                    "\t\tmethod: 'POST',",
                    "\t\tdata: complex,",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidComplexParamPutActionTest()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidComplexParamPutActionConfig(complex: IMyEmptyTestClass): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/configtest/voidcomplexparamput',",
                    "\t\tmethod: 'PUT',",
                    "\t\tdata: complex,",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void VoidComplexParamAndReplaceActionTest()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
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
            });
        }

        [TestMethod]
        public void VoidComplexParamAndReplaceGetActionTest()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
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
            });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ComplexParamAndReplaceGetActionErrorTest()
        {
            RegisterController<ConfigComplexParamGerErrorTestController>();

            RunInScope(() => { VisitActionsFromController<ConfigComplexParamGerErrorTestController>(); });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BigintParamErrorTest()
        {
            RegisterController<ConfigBigintErrorTestController>();

            RunInScope(() => { VisitActionsFromController<ConfigBigintErrorTestController>(); });
        }

        [TestMethod]
        public void DataPropertyForDeleteMethodBodyTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public DeleteComplexParamConfig(param: IComplexDeleteType): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/methodtest/deletedata',",
                    "\t\tmethod: 'DELETE',",
                    "\t\tdata: param,",
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), $"\nExpected: {expectedContent}\nGenerated: {content}");
            });
        }

        [TestMethod]
        public void ParamsPropertyForDeleteMethodUrlTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
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
            });
        }

        [TestMethod]
        public void EnumerableParamsPropertyForDeleteMethodUrlTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
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
            });
        }

        [TestMethod]
        public void ContentTypeSetForDeleteBodyTest()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
                var expectedLines = new List<string>
                {
                    "public DeleteComplexParamConfig(param: IComplexDeleteType): ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: API_BASE_URL + 'api/methodtest/deletedata',",
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
            });
        }

        [TestMethod]
        public void DeleteUrlReplaceAndBody()
        {
            RegisterController<GeneratedMethodTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<GeneratedMethodTestController>();
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
            });
        }
    }
}
