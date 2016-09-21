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
        public void ControllerDescriptionPart_TestControllerHeader()
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

                var controllerDesciptionPart = new ControllerDescriptionPart(controllerDescription);

                apiVisitor.Visit(controllerDesciptionPart);
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
    }
}
