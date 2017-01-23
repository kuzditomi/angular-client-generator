using System;
using System.Collections.Generic;
using AngularClientGeneratorTest.TestControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                    "public VoidParameterlessGetActionConfig() : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/void',",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);
                
                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidStringParameterActionConfig(stringparam: string) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidstring',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tstringparam: stringparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidIntParameterActionConfig(intparam: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidint',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tintparam: intparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        public void VoidDateParameterActionConfig()
        {
            RegisterController<ConfigVoidTestController>();

            RunInScope(() =>
            {
                var content = VisitActionsFromController<ConfigVoidTestController>();
                var expectedLines = new List<string>
                {
                    "public VoidDateParametersActionConfig(from: number, to: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voiddates',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tfrom: from,",
                    "\t\t\tto: to,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidDoubleParameterActionConfig(doubleparam: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voiddouble',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tdoubleparam: doubleparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidDecimalParameterActionConfig(decimalparam: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voiddecimal',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tdecimalparam: decimalparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidGuidParameterActionConfig(guidparam: string) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidguid',",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: {",
                    "\t\t\tguidparam: guidparam,",
                    "\t\t},",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidSimpleParametersActionConfig(a: string, b: number, c: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidsimpleparams',",
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

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidReplaceNumberActionConfig(id: number) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl('api/configtest/voidreplaceparams/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidReplaceMoreParamsActionConfig(id: number, second: string) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl('api/configtest/voidreplacemoreparams/{id}/more/{second}', {",
                    "\t\t\tid: id,",
                    "\t\t\tsecond: second,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidComplexParamPostActionConfig(complex: IMyEmptyTestClass) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidcomplexparampost',",
                    "\t\tmethod: 'POST',",
                    "\t\tdata: complex,",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidComplexParamPutActionConfig(complex: IMyEmptyTestClass) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: 'api/configtest/voidcomplexparamput',",
                    "\t\tmethod: 'PUT',",
                    "\t\tdata: complex,",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidComplexParamAndReplaceActionConfig(id: string, complex: IMyEmptyTestClass) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl('api/configtest/voidcomplexparamandreplace/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'PUT',",
                    "\t\tdata: complex,",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
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
                    "public VoidComplexParamAndReplaceGetActionConfig(id: string, complex: IMyEmptyTestClass) : ng.IRequestConfig {",
                    "\treturn {",
                    "\t\turl: replaceUrl('api/configtest/voidcomplexparamandreplaceget/{id}', {",
                    "\t\t\tid: id,",
                    "\t\t}),",
                    "\t\tmethod: 'GET',",
                    "\t\tparams: complex,",
                    "\t};",
                    "}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nExpected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ComplexParamAndReplaceGetActionErrorTest()
        {
            RegisterController<ConfigComplexParamGerErrorTestController>();

            RunInScope(() =>
            {
                VisitActionsFromController<ConfigComplexParamGerErrorTestController>();
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BigintParamErrorTest()
        {
            RegisterController<ConfigBigintErrorTestController>();

            RunInScope(() =>
            {
                VisitActionsFromController<ConfigBigintErrorTestController>();
            });
        }
    }
}
