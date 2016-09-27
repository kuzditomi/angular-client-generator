using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using System.Web.Http.Description;
using System.Web.Http;
using System.IO;
using AngularClientGeneratorTest.TestControllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using AngularClientGenerator.Config;
using AngularClientGeneratorTest.Util;
using Microsoft.Owin.Hosting;
using Owin;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class GeneratorTest : TestBase
    {
        [TestMethod]
        public void CreateGeneratorDefaultParams()
        {
            var generator = new Generator(this.ApiExplorer);

            Assert.AreEqual("angular-generated-client.ts", generator.Config.ExportPath);
            Assert.AreEqual(Language.TypeScript, generator.Config.Language);
        }

        [TestMethod]
        public void CreateGeneratorOverriddenParams()
        {
            var path = "generate/here/the/code";

            var config = new GeneratorConfig
            {
                ExportPath = path
            };

            var generator = new Generator(this.ApiExplorer)
            {
                Config = config
            };

            Assert.AreEqual(path, generator.Config.ExportPath);
        }

        [TestMethod]
        public void GenerateCode()
        {
            this.HttpConfiguration = new HttpConfiguration();
            this.ApiExplorer = new ApiExplorer(this.HttpConfiguration);

            var generator = new Generator(this.ApiExplorer);
            generator.Generate();

            var fileExists = File.Exists(generator.Config.ExportPath);

            Assert.IsTrue(fileExists);
        }
        
        [TestMethod]
        public void GenerateAllRegisteredControllers()
        {
            RegisterController<TestController>();
            RegisterController<SimpleController>();
            RegisterController<ConfigVoidTestController>();
            RegisterController<GeneratedMethodTestController>();
            RegisterController<TypeTestController>();

            this.RunInScope(() =>
            {
                var generator = new Generator(this.ApiExplorer);
                generator.Generate();

                var content = File.ReadAllText(generator.Config.ExportPath);

                var needToContain = new List<string>
                {
                    "ApiTestService",
                    "ApiSimpleService",
                    "ApiConfigVoidTestService",
                    "ApiGeneratedMethodTestService"
                };

                foreach (var controller in needToContain)
                {
                    Assert.IsTrue(content.Contains(controller), "Generator doesnt include registered controller: " + controller);
                }
            });
        }
    }
}
