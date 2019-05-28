using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    [TestClass]
    public class AngularApiVisitorTest : TsApiVisitorTestBase
    {
        private AngularApiVisitor apiVisitor;

        [TestInitialize]
        public void Init()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = ClientType.Angular
            };

            var builder = new ClientBuilder(config);
            this.apiVisitor = new AngularApiVisitor(config, builder);
        }

        [TestMethod]
        public void ModuleDefinitionTest()
        {
            // Arrange
            var moduleDesciptionPart = new ModuleDescriptionPart
            {
                Name = "MyTestModule",
                ControllerDescriptionParts = new List<ControllerDescriptionPart>()
            };

            // Act
            apiVisitor.Visit(moduleDesciptionPart);
            var content = apiVisitor.GetContent();

            // Assert
            var expectedLines = new List<string> {
                "@NgModule({",
                "\tdeclarations: [",
                "\t],",
                "\timports: [",
                "\t\tHttpClientModule,",
                "\t],",
                "})",
                "export class MyTestModule {",
                "}"
            };
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nAngular module definition is not found or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, content));
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
            apiVisitor.Visit(moduleDesciptionPart);
            var content = apiVisitor.GetContent();

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

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nAngular module definition does not contain services for controllers or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, content));
        }

        [TestMethod]
        public void EmptyServiceDefinitionFromControllerTest()
        {
            // Arrange
            var controllerDescriptionPart = new ControllerDescriptionPart(new ControllerDescriptor
            {
                Name = "MySuperTest",
                ActionDescriptors = new List<ActionDescriptor>()
            });

            // Act
            apiVisitor.Visit(controllerDescriptionPart);
            var content = apiVisitor.GetContent();

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

            Assert.IsTrue(content.Contains(expectedContent), String.Format("\nAngular service definition is not present or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, content));
        }
    }
}
