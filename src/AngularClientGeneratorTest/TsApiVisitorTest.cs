using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using System.Web.Http.Description;
using System.Web.Http;
using System.IO;
using System.Linq;
using System.Web.Http.Controllers;
using AngularClientGeneratorTest.TestControllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGeneratorTest.Util;
using Microsoft.Owin.Hosting;
using Owin;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class TsApiVisitorTest : TestBase
    {
        [TestMethod]
        public void ControllerDescriptionPart_TestController()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
                var builder = new ClientBuilder(config);
                var apiVisitor = new TsApiVisitor(config, builder);

                var controllerDescription = ApiExplorer
                    .ApiDescriptions
                    .First(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "Test")
                    .ActionDescriptor
                    .ControllerDescriptor;

                var actionDescriptions = ApiExplorer
                    .ApiDescriptions
                    .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "Test")
                    .Select(a => new ActionDescriptionPart(a.ActionDescriptor))
                    .ToList();

                var controllerDesciptionPart = new ControllerDescriptionPart(controllerDescription)
                {
                    ActionDescriptionParts = actionDescriptions
                };

                apiVisitor.Visit(controllerDesciptionPart);

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>
                {
                    "export class ApiTestService {",
                    "}",
                    ".factory(ApiTestService)",
                    "static $inject = ['$http', '$q']",
                    "constructor(private http, private q)"
                };

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }

        [TestMethod]
        public void ActionDescriptionPart_TestController_()
        {
            RegisterController<TestController>();

            RunInScope(() =>
            {
                var config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
                var builder = new ClientBuilder(config);
                var apiVisitor = new TsApiVisitor(config, builder);

                var apiDescriptions = ApiExplorer
                    .ApiDescriptions
                    .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "Test")
                    .Select(a => a.ActionDescriptor);

                foreach (var httpActionDescriptor in apiDescriptions)
                {
                    var actionDescriptorPart = new ActionDescriptionPart(httpActionDescriptor);
                    actionDescriptorPart.Accept(apiVisitor);
                }

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>();

                foreach (var apiDescription in apiDescriptions)
                {
                    var methodName = apiDescription.ActionName;
                    expectedContents.Add(String.Format("public {0}", methodName));
                }

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }
    }
}
