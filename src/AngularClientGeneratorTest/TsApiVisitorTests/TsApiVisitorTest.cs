﻿using System;
using System.Collections.Generic;
using System.Linq;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGeneratorTest.TestControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGeneratorTest.TsApiVisitorTests
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
                    .Select(a => new ActionDescriptionPart(a))
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
                    ".service('ApiTestService', ApiTestService)",
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
        public void ActionDescriptionPart_TestController()
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
                    .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "Test");

                foreach (var httpActionDescriptor in apiDescriptions)
                {
                    var actionDescriptorPart = new ActionDescriptionPart(httpActionDescriptor);
                    actionDescriptorPart.Accept(apiVisitor);
                }

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>();

                foreach (var apiDescription in apiDescriptions)
                {
                    var methodName = apiDescription.ActionDescriptor.ActionName;
                    expectedContents.Add(String.Format("public {0}", methodName));
                }

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }

        [TestMethod]
        public void TypeDescriptionPart_TestController()
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
                    .ActionDescriptor.ControllerDescriptor;

                var actionDescriptions = ApiExplorer
                    .ApiDescriptions
                    .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == "Test");

                var moduleDescription = new ModuleDescriptionPart
                {
                    ControllerDescriptionParts = new List<ControllerDescriptionPart>
                    {
                        new ControllerDescriptionPart(controllerDescription)
                        {
                            ActionDescriptionParts = actionDescriptions.Select(a => new ActionDescriptionPart(a))
                        }
                    }
                };


                moduleDescription.Accept(apiVisitor);

                var content = apiVisitor.GetContent();

                var expectedContents = new List<string>
                {
                    "export interface ITestModelA {",
                    "export interface ITestModelB {",
                };

                foreach (var expectedContent in expectedContents)
                {
                    Assert.IsTrue(content.Contains(expectedContent), "Generated content is not included: {0}", expectedContent);
                }
            });
        }
    }
}