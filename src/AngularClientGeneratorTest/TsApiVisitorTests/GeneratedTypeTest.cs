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
                    "\texport interface IMyEmptyTestClass { }",
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
    }
}
