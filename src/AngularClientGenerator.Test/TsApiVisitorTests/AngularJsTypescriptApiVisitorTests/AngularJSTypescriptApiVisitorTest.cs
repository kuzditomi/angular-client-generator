using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGenerator.Contracts.Descriptors;
using System.Net.Http;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class AngularJSTypescriptApiVisitorTest : TsApiVisitorTestsBase
    {
        public AngularJSTypescriptApiVisitorTest() : base(ClientType.AngularJsTypeScript)
        {
        }

        [TestMethod]
        public void ControllerDescriptionPartTest()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                ClientType = ClientType.AngularJsTypeScript
            };
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);
            var controllerDesciptionPart = new ControllerDescriptionPart(new ControllerDescriptor
            {
                Name = "Test",
                ActionDescriptors = new List<ActionDescriptor>()
            });

            apiVisitor.Visit(controllerDesciptionPart);

            var content = apiVisitor.GetContent();

            var expectedContents = new List<string> {
                "export class ApiTestService {",
                "}",
                ".service('ApiTestService', ApiTestService)",
                "static $inject = ['$http', '$q']",
                "constructor(private http: ng.IHttpService, private q: ng.IQService)"
            };

            foreach (var expectedContent in expectedContents)
            {
                Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            }
        }

        [TestMethod]
        public void ActionDescriptionPartTest()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.Tab,
                ClientType = ClientType.AngularJsTypeScript
            };
            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

            var actionDescriptors = new string[] { "A", "B", "Random" }.Select(name => new ActionDescriptor
            {
                Name = name,
                HttpMethod = HttpMethod.Get,
                ParameterDescriptors = Enumerable.Empty<ParameterDescriptor>(),
                ReturnValueDescriptor = new TypeDescriptor { Type = typeof(string) },
                UrlTemplate = string.Empty
            });

            foreach (var actionDescriptor in actionDescriptors)
            {
                var actionDescriptorPart = new ActionDescriptionPart(actionDescriptor);
                actionDescriptorPart.Accept(apiVisitor);
            }

            var content = apiVisitor.GetContent();

            var expectedMethods = new string[] { "A", "B", "Random" };
            var expectedContents = new List<string>();

            foreach (var methodName in expectedMethods)
            {
                expectedContents.Add(String.Format("public {0}", methodName));
            }

            foreach (var expectedContent in expectedContents)
            {
                Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
            }
        }
    }
}
