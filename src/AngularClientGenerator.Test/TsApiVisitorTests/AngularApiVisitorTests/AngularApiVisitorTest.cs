using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularApiVisitorTests
{
    [TestClass]
    public class AngularApiVisitorTest : ApiVisitorTestsBase
    {
        public AngularApiVisitorTest() : base(ClientType.Angular)
        {
        }

        [TestMethod]
        public void HeaderCodeTest()
        {
            // Act
            var actualContent = this.visitor.VisitEmptyModule();

            // Assert
            var expectedLines = new List<string> {
                "import { Injectable, NgModule } from '@angular/core';",
                "import { HttpClientModule, HttpClient, HttpErrorResponse } from '@angular/common/http';",
                "import { Observable, throwError } from 'rxjs';",
                "import { catchError } from 'rxjs/operators';",
                "",
                "type RequestOptions = Parameters<HttpClient[\"request\"]>[\"2\"] & { method: string, url: string };"
            };

            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(actualContent.Contains(expectedContent), String.Format("\nHeader imports are not found. Expected: {0}\nGenerated: {1}", expectedContent, actualContent));
        }

        [TestMethod]
        public void ModuleDefinitionTest()
        {
            // Act
            var actualContent = this.visitor.VisitEmptyModule();

            // Assert
            var expectedLines = new List<string> {
                "\t@NgModule({",
                "\t\tproviders: [",
                "\t\t],",
                "\t\timports: [",
                "\t\t\tHttpClientModule,",
                "\t\t],",
                "\t})",
                "\texport class example {",
                "\t}"
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
                "\t@NgModule({",
                "\t\tproviders: [",
                "\t\t\tTestAApiService,",
                "\t\t\tTestBApiService,",
                "\t\t],",
                "\t\timports: [",
                "\t\t\tHttpClientModule,",
                "\t\t],",
                "\t})",
                "\texport class MyTestModule {",
                "\t}"
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
            var actualContent = this.visitor.VisitController(controllerDescriptor);

            // Assert
            var expectedLines = new List<string> {
                "@Injectable({",
                "\tprovidedIn: 'root'",
                "})",
                "export class MySuperTestApiService {",
                "\tapiUrl: string = API_BASE_URL;",
                "",
                "\tconstructor(private httpClient: HttpClient) {",
                "\t}",
                "",
                "}"
            };
            var expectedContent = String.Join(Environment.NewLine, expectedLines);

            Assert.IsTrue(actualContent.Contains(expectedContent), String.Format("\nAngular service definition is not present or incorrect. Expected: {0}\nGenerated: {1}", expectedContent, actualContent));
        }
    }
}
