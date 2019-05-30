using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularApiVisitorTests
{
    [TestClass]
    public class AngularApiVisitorTest : TsApiVisitorTestsBase
    {
        public AngularApiVisitorTest() : base(ClientType.Angular)
        {
        }

        [TestMethod]
        public void ModuleDefinitionTest()
        {
            // Act
            var actualContent = this.visitor.VisitEmptyModule();
            
            // Assert
            var expectedLines = new List<string> {
                "@NgModule({",
                "\tdeclarations: [",
                "\t],",
                "\timports: [",
                "\t\tHttpClientModule,",
                "\t],",
                "})",
                "export class example {",
                "}"
            };
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(actualContent.Contains(expectedContent), String.Format("\nAngular module definition is not found or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, actualContent));
        }

        [TestMethod]
        public void ModuleDefinitionContainsServicesTest()
        {
            // Arrange
            var moduleDesciptionPart = new ModuleDescriptionPart
            {
                Name = "MyTestModule",
                ControllerDescriptionParts = new List<ControllerDescriptionPart>
                    {
                        new ControllerDescriptionPart(new ControllerDescriptor
                        {
                            Name = "TestA",
                            ActionDescriptors = new List<ActionDescriptor>()
                        }),
                        new ControllerDescriptionPart(new ControllerDescriptor
                        {
                            Name = "TestB",
                            ActionDescriptors = new List<ActionDescriptor>()
                        }),
                    }
            };

            // Act
            var actualContent = this.visitor.VisitModule(moduleDesciptionPart);

            // Assert
            var expectedLines = new List<string> {
                "@NgModule({",
                "\tdeclarations: [",
                "\t\tTestAApiService,",
                "\t\tTestBApiService,",
                "\t],",
                "\timports: [",
                "\t\tHttpClientModule,",
                "\t],",
                "})",
                "export class MyTestModule {",
                "}"
            };
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(actualContent.Contains(expectedContent), String.Format("\nAngular module definition does not contain services for controllers or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, actualContent));
        }

        [TestMethod]
        public void EmptyServiceDefinitionFromControllerTest()
        {
            // Arrange
            var controllerDescriptor = new ControllerDescriptor
            {
                Name = "MySuperTest",
                ActionDescriptors = new List<ActionDescriptor>()
            };

            // Act
            var actualContent = this.visitor.VisitTsController(controllerDescriptor);

            // Assert
            var expectedLines = new List<string> {
                "@Injectable()",
                "export class MySuperTestApiService {",
                "\tapiUrl: string = API_BASE_URL;",
                "",
                "\tconstructor(private http: HttpClient) {",
                "\t}",
                "}"
            };
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(actualContent.Contains(expectedContent), String.Format("\nAngular service definition is not present or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, actualContent));
        }
    }
}
