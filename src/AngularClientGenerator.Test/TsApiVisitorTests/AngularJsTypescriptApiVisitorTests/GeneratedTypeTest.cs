using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Contracts.Descriptors;
using System.Net.Http;
using System.Collections.Generic;
using System;
using AngularClientGenerator.Test.TestModels;
using System.Threading.Tasks;
using AngularClientGenerator.Config;
using AngularClientGenerator.Test.Utils;
using AngularClientGenerator.Contracts;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class GeneratedTypeTest : TestBaseWithHelper
    {
        public GeneratedTypeTest() : base(ClientType.AngularJsTypeScript)
        {
        }

        [TestMethod]
        public void BasicTypesNotGeneratedConfig()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "BasicTypesAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "s",
                        ParameterType = typeof(string)
                    },
                    new ParameterDescriptor
                    {
                        ParameterName = "a",
                        ParameterType = typeof(int)
                    },
                    new ParameterDescriptor
                    {
                        ParameterName = "b",
                        ParameterType = typeof(double)
                    },
                    new ParameterDescriptor
                    {
                        ParameterName = "c",
                        ParameterType = typeof(decimal)
                    },
                    new ParameterDescriptor
                    {
                        ParameterName = "d",
                        ParameterType = typeof(float)
                    },
                    new ParameterDescriptor
                    {
                        ParameterName = "f",
                        ParameterType = typeof(bool)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var expectedHeader =
                "public BasicTypesAction = (s: string, a: number, b: number, c: number, d: number, f: boolean): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(expectedHeader), "BasicTypesAction is not generated, or it's wrong.");

            var basicTypes = new[] { "string", "number", "boolean" };

            foreach (var basicType in basicTypes)
            {
                Assert.IsFalse(content.Contains($"interface {basicType}"), $"{basicType} type should not be generated as new type");
            }
        }

        [TestMethod]
        public void NoDoubleDeclaration()
        {
            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor>
                {
                    new ActionDescriptor
                    {
                        Name = "OneMyEmptyTestClassAction",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "a",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(MyEmptyTestClass)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    },
                    new ActionDescriptor
                    {
                        Name = "TwoMyEmptyTestClassAction",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "b",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(MyEmptyTestClass)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    }
                }
            };

            var content = this.visitor.VisitTsControllerInModule(controllerDescriptor);

            var methodHeader1 = "public OneMyEmptyTestClassAction = (model: IMyEmptyTestClass): ng.IPromise<void> => {";
            var methodHeader2 = "public TwoMyEmptyTestClassAction = (model: IMyEmptyTestClass): ng.IPromise<void> => {";

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
        }

        [TestMethod]
        public void EnumsGeneratedAsEnums()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "EnumTypeAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "enumvalue",
                        ParameterType = typeof(TestEnum)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public EnumTypeAction = (enumvalue: TestEnum): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "Enum type method header is not present, or incorrect.");

            var expectedLines = new List<string>
                {
                    "\texport enum TestEnum {",
                    "\t\tOne = 0,",
                    "\t\tTwo = 1,",
                    "\t\tThree = 2,",
                    "\t}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nEnum is not generated. Expected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void EnumsValueGenerated()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "NumberedEnumTypeAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "enumvalue",
                        ParameterType = typeof(TestNumberedEnum)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public NumberedEnumTypeAction = (enumvalue: TestNumberedEnum): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "Enum type method header is not present, or incorrect.");

            var expectedLines = new List<string>
                {
                    "\texport enum TestNumberedEnum {",
                    "\t\tZero = 0,",
                    "\t\tThree = 3,",
                    "\t\tTen = 10,",
                    "\t}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("TestNumberedEnum is not generated propertly. Expected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void NoArrayGenerated()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "ArrayTypeAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "arr",
                        ParameterType = typeof(string[])
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public ArrayTypeAction = (arr: string[]): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "Array type method header is not present, or incorrect.");

            Assert.IsFalse(content.Contains("public interface string[]"), "Array types should not be defined as new type");
        }

        [TestMethod]
        public void TypesDiscoveredRescursively()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "TestRecursiveDiscovery",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "model",
                        ParameterType = typeof(TestComplexType)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public TestRecursiveDiscovery = (model: ITestComplexType): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "TestRecursiveDiscovery method header is not present, or incorrect.");

            Assert.IsTrue(content.Contains("export interface ITestComplexType"), "ITestComplexType should be defined as new type");
            Assert.IsTrue(content.Contains("export interface ITestComplexInnerProperty"), "ITestComplexInnerProperty should be defined as new type, recursive discovery is not working");
        }

        [TestMethod]
        public void DiscoveredIfElementOnly()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "ArrayTypesAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "model",
                        ParameterType = typeof(ArrayOnlyType[])
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);
            var methodHeader = "public ArrayTypesAction = (model: IArrayOnlyType[]): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "ArrayTypesAction method header is not present, or incorrect.");

            Assert.IsTrue(content.Contains("export interface IArrayOnlyType {"), "IArrayOnlyType should be defined as new type");
        }

        [TestMethod]
        public void DiscoveredIfOptionalOnly()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "OnlyAsOptionalParam",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        IsOptional = true,
                        ParameterName = "model",
                        ParameterType = typeof(AsOptionalParamOnly)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public OnlyAsOptionalParam = (model?: IAsOptionalParamOnly): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "OnlyAsOptionalParam method header is not present, or incorrect.");

            Assert.IsTrue(content.Contains("export interface IAsOptionalParamOnly {"), "IAsOptionalParamOnly should be defined as new type");
        }

        [TestMethod]
        public void DiscoveredIfGenericOnly()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "EnumerableTypeAction",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "model",
                        ParameterType = typeof(IEnumerable<EnumerableOnlyType>)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public EnumerableTypeAction = (model: IEnumerableOnlyType[]): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "EnumerableTypeAction method header is not present, or incorrect.");

            Assert.IsTrue(content.Contains("export interface IEnumerableOnlyType {"), "IEnumerableOnlyType should be defined as new type");
        }

        [TestMethod]
        public void NullablePropertyTypeTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "NullableProperty",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "hasnullable",
                        ParameterType = typeof(ContainsNullableProperty)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public NullableProperty = (hasnullable: IContainsNullableProperty): ng.IPromise<void> => {";

            Assert.IsTrue(content.Contains(methodHeader), "NullableProperty method header is not present, or incorrect.");

            var expectedLines = new List<string>
                {
                    "\texport interface IContainsNullableProperty {",
                    "\t\tIntableNull?: number;",
                    "\t}"
                };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nNullable property is not generated or wrong. Expected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void OptionalParamTest()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "OptionalParam",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        IsOptional = true,
                        ParameterName = "optional",
                        ParameterType = typeof(int?)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public OptionalParam = (optional?: number): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeader), "OptionalParam method header is not present, or incorrect.");

            var configHeader = "public OptionalParamConfig(optional?: number): ng.IRequestConfig {";
            Assert.IsTrue(content.Contains(configHeader), "OptionalParamConfig method header is not present, or incorrect.");

            Assert.IsFalse(content.Contains("export interface INullable"), "optional parameters should not create nullable types");
        }

        [TestMethod]
        public void TestComplexTypeFullyDeclared()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "OptionalParam",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "optional",
                        ParameterType = typeof(TestComplexType)
                    },
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var interfaceDeclaration = string.Join(Environment.NewLine, new[]
            {
                    "\texport interface ITestComplexType {",
                    "\t\tNormalProperty: string;",
                    "\t\tComplexProperty: ITestComplexInnerProperty;",
                    "\t\tNumberProperty: number;",
                    "\t\tEnum: TestEnum;",
                    "\t}"
                });

            Assert.IsTrue(content.Contains(interfaceDeclaration), "ITestComplexType should be defined fully");
        }

        [TestMethod]
        public void ActionResultToAny()
        {
            var mockIHttpActionResultType = new Moq.Mock<Type>();
            mockIHttpActionResultType.Setup(m => m.Name).Returns("IHttpActionResult");

            var actionDescriptor = new ActionDescriptor
            {
                Name = "ActionResultWithoutAttribute",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = mockIHttpActionResultType.Object }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public ActionResultWithoutAttribute = (): ng.IPromise<any> => {";
            Assert.IsTrue(content.Contains(methodHeader), "ActionResultWithoutAttribute method header is not present, or incorrect.");

            var interfaceDeclaration = "export interface IIHttpActionResult";
            var interfaceDeclaration2 = "export interface any";

            Assert.IsFalse(content.Contains(interfaceDeclaration), "IHttpActionResult shouldnt be declared as type");
            Assert.IsFalse(content.Contains(interfaceDeclaration2), "any shouldnt be declared as type");
        }

        [TestMethod]
        public void UnWrapTaskGeneric()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "UnWrapTaskGeneric",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(Task<int>) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public UnWrapTaskGeneric = (): ng.IPromise<number> => {";
            Assert.IsTrue(content.Contains(methodHeader), "UnWrapTaskGeneric method header is not present, or incorrect.");

            var interfaceDeclaration = "export interface ITask";

            Assert.IsFalse(content.Contains(interfaceDeclaration), "ITask shouldnt be declared as type");
        }

        [TestMethod]
        public void TaskToVoid()
        {
            var mockIHttpActionResultType = new Moq.Mock<Type>();
            mockIHttpActionResultType.Setup(m => m.Name).Returns("IHttpActionResult");

            var actionDescriptor = new ActionDescriptor
            {
                Name = "TaskToVoid",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(Task) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public TaskToVoid = (): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeader), "TaskToVoid method header is not present, or incorrect.");

            var interfaceDeclaration = "export interface ITask";
            var interfaceDeclaration2 = "export interface void";

            Assert.IsFalse(content.Contains(interfaceDeclaration), "ITask shouldnt be declared as type");
            Assert.IsFalse(content.Contains(interfaceDeclaration2), "void shouldnt be declared as type");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SameNameFromDifferentNameSpaceWithoutNamespacesEnabled()
        {
            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor>
                {
                    new ActionDescriptor
                    {
                        Name = "A",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "a",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(TestModels.NameSpaceA.SameNameDifferentNameSpace)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    },
                    new ActionDescriptor
                    {
                        Name = "B",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "b",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(TestModels.NameSpaceB.SameNameDifferentNameSpace)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    }
                }
            };

            this.visitor.VisitTsControllerInModule(controllerDescriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SameNameFromDifferentNameSpaceWithNamespacesEnabledWithoutNamingAction()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true
            };

            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor>
                {
                    new ActionDescriptor
                    {
                        Name = "A",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "a",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(TestModels.NameSpaceA.SameNameDifferentNameSpace)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    },
                    new ActionDescriptor
                    {
                        Name = "B",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "b",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(TestModels.NameSpaceB.SameNameDifferentNameSpace)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    }
                }
            };

            this.visitor.VisitTsControllerInModule(controllerDescriptor, config);
        }

        [TestMethod]
        public void SameNameFromDifferentNameSpaceWithNamespacesEnabledWithNamingAction()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor>
                {
                    new ActionDescriptor
                    {
                        Name = "SameNameDifferentNameSpaceA",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "a",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "parameter",
                                ParameterType = typeof(TestModels.NameSpaceA.SameNameDifferentNameSpace)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    },
                    new ActionDescriptor
                    {
                        Name = "SameNameDifferentNameSpaceB",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "b",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "parameter",
                                ParameterType = typeof(TestModels.NameSpaceB.SameNameDifferentNameSpace)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    }
                }
            };

            var content = this.visitor.VisitTsControllerInModule(controllerDescriptor, config);

            var methodHeadera = "public SameNameDifferentNameSpaceA = (parameter: TestModels.NameSpaceA.ISameNameDifferentNameSpace): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeadera), "SameNameDifferentNameSpaceA method header is not present, or incorrect.");

            var methodHeaderb = "public SameNameDifferentNameSpaceB = (parameter: TestModels.NameSpaceB.ISameNameDifferentNameSpace): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeaderb), "SameNameDifferentNameSpaceB method header is not present, or incorrect.");

            var interfaceDeclaration = "export interface ISameNameDifferentNameSpace";
            var occurances = content.Split(new[] { interfaceDeclaration }, StringSplitOptions.None).Count() - 1;

            Assert.AreEqual(2, occurances, "SameNameDifferentNameSpace type declaration should be present exactly once.");

            var namespaceADeclaration = "export namespace TestModels.NameSpaceA";
            var namespaceBDeclaration = "export namespace TestModels.NameSpaceB";

            Assert.IsTrue(content.Contains(namespaceADeclaration));
            Assert.IsTrue(content.Contains(namespaceBDeclaration));
        }

        [TestMethod]
        public void DateTimeReplaced()
        {
            var actionDescriptor = new ActionDescriptor
            {
                Name = "DateTimeReplaced",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "date",
                        ParameterType = typeof(DateTime)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor);

            var methodHeader = "public DateTimeReplaced = (date: string): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeader), "DateTimeReplaced method header is not present, or incorrect.");

            var interfaceDeclaration = "export interface IDateTime";
            Assert.IsFalse(content.Contains(interfaceDeclaration), "DateTime shouldnt be declared as type");
        }

        [TestMethod]
        public void GenericType()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "GenericType",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "type",
                        ParameterType = typeof(GenericClass<int>)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var methodHeader = "public GenericType = (type: TestModels.IGenericClass<number>): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeader), "GenericType method header is not present, or incorrect.");

            var interfaceDeclaration = string.Join(Environment.NewLine, new[]
            {
                    "\t\texport interface IGenericClass<T> {",
                    "\t\t\tGenericList: T[];",
                    "\t\t\tGenericProperty: T;",
                    "\t\t}"
                });

            Assert.IsTrue(content.Contains(interfaceDeclaration), "IGenericClass<T> is not declared, or incorrect");
        }

        [TestMethod]
        public void GenericTypeOnlyOnce()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "GenericType",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "type",
                        ParameterType = typeof(GenericClass<int>)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var interfaceDeclaration = "export interface IGenericClass<T>";
            var numberOfDeclarations = content.Occures(interfaceDeclaration);

            Assert.AreEqual(1, numberOfDeclarations, $"IGenericClass<T> should be declared exactly once, declared {numberOfDeclarations} times instead");
        }

        [TestMethod]
        public void NamespacedArrays()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "NameSpacedArrays",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "parameter",
                        ParameterType = typeof(TestModels.NameSpaceA.SameNameDifferentNameSpace[])
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var methodHeadera = "public NameSpacedArrays = (parameter: TestModels.NameSpaceA.ISameNameDifferentNameSpace[]): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeadera), "SameNameDifferentNameSpaceA method header is not present, or incorrect.");
        }

        [TestMethod]
        public void NamespacedProperties()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "NamespacedProperties",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "parameter",
                        ParameterType = typeof(HasNameSpacedProperties)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var methodHeadera = "public NamespacedProperties = (parameter: TestModels.IHasNameSpacedProperties): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeadera), "NamespacedProperties method header is not present, or incorrect.");

            var interfaceDeclaration = string.Join(Environment.NewLine, new[]
            {
                "\t\texport interface IHasNameSpacedProperties {",
                "\t\t\tAProperty: TestModels.NameSpaceA.ISameNameDifferentNameSpace;",
                "\t\t\tBProperty: TestModels.NameSpaceB.ISameNameDifferentNameSpace;",
                "\t\t}"
            });

            Assert.IsTrue(content.Contains(interfaceDeclaration), "HasNameSpacedProperties should have namespaced properties");
        }

        [TestMethod]
        public void NamespacedEnum()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "NamespacedEnum",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "enumparam",
                        ParameterType = typeof(TestModels.EnumNameSpace.OnlyEnumInNameSpace)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var methodHeadera = "public NamespacedEnum = (enumparam: TestModels.EnumNameSpace.OnlyEnumInNameSpace): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeadera), "NamespacedEnum method header is not present, or incorrect.");

            var interfaceDeclaration = string.Join(Environment.NewLine, new[]
            {
                "\texport namespace TestModels.EnumNameSpace {",
                "\t\texport enum OnlyEnumInNameSpace {",
                "\t\t\tOne = 0,",
                "\t\t\tTwo = 1,",
                "\t\t}",
                "\t}"
            });

            Assert.IsTrue(content.Contains(interfaceDeclaration), "HasNameSpacedProperties should have namespaced properties");
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string>), "string", "string")]
        [DataRow(typeof(Dictionary<int, string>), "number", "string")]
        [DataRow(typeof(Dictionary<int, DictionaryReturnType>), "number", "TestModels.IDictionaryReturnType")]
        public void DictionaryTypes(Type type, string keyType, string valueType)
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = t => t.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "DictionaryTypeTest",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "dictionary",
                        ParameterType = type
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var methodHeader = $"public DictionaryTypeTest = (dictionary: {{[key: {keyType}]: {valueType}}}): ng.IPromise<void> => {{";
            Assert.IsTrue(content.Contains(methodHeader), "DictionaryTypeTest method header is not present, or incorrect.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DictionaryWithBoolKey()
        {
            DictionaryTypes(typeof(Dictionary<bool, string>), "foo", "bar");
        }

        [TestMethod]
        public void DictionaryValueTypeVisited()
        {
            var config = new GeneratorConfig
            {
                UseNamespaces = true,
                NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Test.", "")
            };

            var actionDescriptor = new ActionDescriptor
            {
                Name = "DictionaryWithComplexValue",
                HttpMethod = HttpMethod.Get,
                UrlTemplate = "a",
                ParameterDescriptors = new List<ParameterDescriptor>
                {
                    new ParameterDescriptor
                    {
                        ParameterName = "dictionary",
                        ParameterType = typeof(Dictionary<int, DictionaryReturnType>)
                    }
                },
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
            };

            var content = this.visitor.VisitTsActionInModule(actionDescriptor, config);

            var methodHeader = "public DictionaryWithComplexValue = (dictionary: {[key: number]: TestModels.IDictionaryReturnType}): ng.IPromise<void> => {";
            Assert.IsTrue(content.Contains(methodHeader), "DictionaryWithComplexValue method header is not present, or incorrect.");

            var interfaceDeclaration = string.Join(Environment.NewLine, new[]
            {
                "\t\texport interface IDictionaryReturnType {",
                "\t\t}"
            });

            Assert.IsTrue(content.Contains(interfaceDeclaration), "IDictionaryReturnType should have been generated");
        }

        [TestMethod]
        public void GenericWithManyTypes()
        {
            var expectedTypes = new List<string>
                {
                    "export interface IGenericClass<T> {",
                    "export interface ITestModelA {",
                    "export interface ITestModelB {"
                };
            var config = new GeneratorConfig { UseNamespaces = false };

            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "ExampleController",
                ActionDescriptors = new List<ActionDescriptor>
                {
                    new ActionDescriptor
                    {
                        Name = "OneMyEmptyTestClassAction",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "a",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(GenericClass<TestModelA>)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(void) }
                    },
                    new ActionDescriptor
                    {
                        Name = "TwoMyEmptyTestClassAction",
                        HttpMethod = HttpMethod.Get,
                        UrlTemplate = "b",
                        ParameterDescriptors = new List<ParameterDescriptor>
                        {
                            new ParameterDescriptor
                            {
                                ParameterName = "model",
                                ParameterType = typeof(MyEmptyTestClass)
                            },
                        },
                        ReturnValueDescriptor = new TypeDescriptor { Type = typeof(GenericClass<TestModelB>) }
                    }
                }
            };


            var content = this.visitor.VisitTsControllerInModule(controllerDescriptor, config);

            foreach (var expectedContent in expectedTypes)
            {
                var occurance = content.Occures(expectedContent);
                Assert.IsTrue(occurance == 1, "Generated content is not included: {0}", expectedContent);
            }
        }
    }
}
