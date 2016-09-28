using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using AngularClientGenerator;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGeneratorTest.TestControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGeneratorTest.TsApiVisitorTests
{
    [TestClass]
    public class GeneratedTypeTest : TestBase
    {
        [TestMethod]
        public void BasicTypesNotGeneratedConfig()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var expectedHeader =
                    "public BasicTypesAction(s: string, a: number, b: number, c: number, d: number, f: boolean) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(expectedHeader), "BasicTypesAction is not generated, or it's wrong.");

                var basicTypes = new[] { "string", "number", "boolean" };

                foreach (var basicType in basicTypes)
                {
                    Assert.IsFalse(content.Contains($"interface {basicType}"), $"{basicType} type should not be generated as new type");
                }
            });
        }

        [TestMethod]
        public void NoDoubleDeclaration()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader1 = "public OneMyEmptyTestClassAction(model: IMyEmptyTestClass) : ng.IPromise<void> {";
                var methodHeader2 = "public TwoMyEmptyTestClassAction(model: IMyEmptyTestClass) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader1), "method header is not present, or incorrect." + "OneMyEmptyTestClassAction");
                Assert.IsTrue(content.Contains(methodHeader2), "method header is not present, or incorrect." + "TwoMyEmptyTestClassAction");

                var expectedLines = new List<string>
                {
                    "\texport interface IMyEmptyTestClass {",
                    "\t}",
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);
                var occurances = content.Split(new[] { expectedContent }, StringSplitOptions.None).Count() - 1;

                Assert.AreEqual(1, occurances, "Type declaration should be present exactly once.");
            });
        }

        [TestMethod]
        public void EnumsGeneratedAsEnums()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public EnumTypeAction(enumvalue: TestEnum) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "Enum type method header is not present, or incorrect.");

                var expectedLines = new List<string>
                {
                    "\texport enum TestEnum {",
                    "\t\tOne,",
                    "\t\tTwo,",
                    "\t\tThree,",
                    "\t}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nEnum is not generated. Expected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void NoArrayGenerated()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public ArrayTypeAction(arr: string[]) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "Array type method header is not present, or incorrect.");

                Assert.IsFalse(content.Contains("public interface string[]"), "Array types should not be defined as new type");
            });
        }

        [TestMethod]
        public void TypesDiscoveredRescursively()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public TestRecursiveDiscovery(model: ITestComplexType) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "TestRecursiveDiscovery method header is not present, or incorrect.");

                Assert.IsTrue(content.Contains("export interface ITestComplexType"), "ITestComplexType should be defined as new type");
                Assert.IsTrue(content.Contains("export interface ITestComplexInnerProperty"), "ITestComplexInnerProperty should be defined as new type, recursive discovery is not working");
            });
        }

        [TestMethod]
        public void DiscoveredIfElementOnly()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public ArrayTypesAction(model: IArrayOnlyType[]) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "ArrayTypesAction method header is not present, or incorrect.");

                Assert.IsTrue(content.Contains("export interface IArrayOnlyType {"), "IArrayOnlyType should be defined as new type");
            });
        }

        [TestMethod]
        public void DiscoveredIfOptionalOnly()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public OnlyAsOptionalParam(model?: IAsOptionalParamOnly) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "OnlyAsOptionalParam method header is not present, or incorrect.");

                Assert.IsTrue(content.Contains("export interface IAsOptionalParamOnly {"), "IAsOptionalParamOnly should be defined as new type");
            });
        }

        [TestMethod]
        public void DiscoveredIfGenericOnly()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public EnumerableTypeAction(model: IEnumerableOnlyType[]) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "EnumerableTypeAction method header is not present, or incorrect.");

                Assert.IsTrue(content.Contains("export interface IEnumerableOnlyType {"), "IEnumerableOnlyType should be defined as new type");
            });
        }

        [TestMethod]
        public void NullablePropertyTypeTest()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public NullableProperty(hasnullable: IContainsNullableProperty) : ng.IPromise<void> {";

                Assert.IsTrue(content.Contains(methodHeader), "NullableProperty method header is not present, or incorrect.");

                var expectedLines = new List<string>
                {
                    "\texport interface IContainsNullableProperty {",
                    "\t\tIntableNull?: number;",
                    "\t}"
                };

                var expectedContent = String.Join(Environment.NewLine, expectedLines);

                Assert.IsTrue(content.Contains(expectedContent), String.Format("\nNullable property is not generated or wrong. Expected: {0}\nGenerated: {1}", expectedContent, content));
            });
        }

        [TestMethod]
        public void OptionalParamTest()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public OptionalParam(optional?: number) : ng.IPromise<void> {";
                Assert.IsTrue(content.Contains(methodHeader), "OptionalParam method header is not present, or incorrect.");

                var configHeader = "public OptionalParamConfig(optional?: number) : ng.IRequestConfig {";
                Assert.IsTrue(content.Contains(configHeader), "OptionalParamConfig method header is not present, or incorrect.");

                Assert.IsFalse(content.Contains("export interface INullable"), "optional parameters should not create nullable types");
            });
        }

        [TestMethod]
        public void TestComplexTypeFullyDeclared()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var interfaceDeclaration = string.Join(Environment.NewLine, new []
                {
                    "\texport interface ITestComplexType {",
                    "\t\tNormalProperty: string;",
                    "\t\tComplexProperty: ITestComplexInnerProperty;",
                    "\t\tNumberProperty: number;",
                    "\t\tEnum: TestEnum;",
                    "\t}"
                });

                Assert.IsTrue(content.Contains(interfaceDeclaration), "ITestComplexType should be defined fully");
            });
        }

        [TestMethod]
        public void ActionResultToAny()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public ActionResultWithoutAttribute() : ng.IPromise<any> {";
                Assert.IsTrue(content.Contains(methodHeader), "ActionResultWithoutAttribute method header is not present, or incorrect.");
                
                var interfaceDeclaration = "export interface IIHttpActionResult";
                var interfaceDeclaration2 = "export interface any";

                Assert.IsFalse(content.Contains(interfaceDeclaration), "IHttpActionResult shouldnt be declared as type");
                Assert.IsFalse(content.Contains(interfaceDeclaration2), "any shouldnt be declared as type");
            });
        }

        [TestMethod]
        public void UnWrapTaskGeneric()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public UnWrapTaskGeneric() : ng.IPromise<any> {";
                Assert.IsTrue(content.Contains(methodHeader), "UnWrapTaskGeneric method header is not present, or incorrect.");

                var interfaceDeclaration = "export interface ITask";

                Assert.IsFalse(content.Contains(interfaceDeclaration), "ITask shouldnt be declared as type");
            });
        }

        [TestMethod]
        public void TaskToVoid()
        {
            RegisterController<TypeTestController>();

            RunInScope(() =>
            {
                var content = VisitModuleWithController<TypeTestController>();

                var methodHeader = "public TaskToVoid() : ng.IPromise<void> {";
                Assert.IsTrue(content.Contains(methodHeader), "TaskToVoid method header is not present, or incorrect.");

                var interfaceDeclaration = "export interface ITask";
                var interfaceDeclaration2 = "export interface void";

                Assert.IsFalse(content.Contains(interfaceDeclaration), "ITask shouldnt be declared as type");
                Assert.IsFalse(content.Contains(interfaceDeclaration2), "void shouldnt be declared as type");
            });
        }
    }
}
