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
        [TestMethod]
        public void ModuleDescriptionPart_TestController()
        {
            // Arrange
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                Language = ClientType.Angular
            };

            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularApiVisitor(config, builder);
            var moduleDesciptionPart = new ModuleDescriptionPart
            {
                Name = "MyTestModule",
                ControllerDescriptionParts = new List<ControllerDescriptionPart>
                    {
                        new ControllerDescriptionPart(new ControllerDescriptor
                        {
                            Name = "Test",
                            ActionDescriptors = new List<ActionDescriptor>()
                        })
                    }
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
    }
}
