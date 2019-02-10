using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Exceptions;
using AngularClientGeneratorTest.TestControllers;
using AngularClientGenerator.Descriptor;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class GeneratorTest : TestBase
    {
        [TestMethod]
        public void CreateGeneratorDefaultParams()
        {
            var generator = new Generator(null);

            Assert.AreEqual("angular-generated-client.ts", generator.Config.ExportPath);
            Assert.AreEqual(Language.TypeScript, generator.Config.Language);
            Assert.AreEqual(false, generator.Config.UseNamespaces);
            Assert.AreEqual(IndentType.Tab, generator.Config.IndentType);
            Assert.AreEqual("mymodule", generator.Config.ModuleName);
        }

        [TestMethod]
        public void CreateGeneratorOverriddenParams()
        {
            var path = "generate/here/the/code";

            var config = new GeneratorConfig
            {
                ExportPath = path
            };

            var generator = new Generator(null)
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
            var descriptor = ApiDescriptorConverter.CreateApiDescriptor(this.ApiExplorer);
            var generator = new Generator(descriptor);
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
                var descriptor = ApiDescriptorConverter.CreateApiDescriptor(this.ApiExplorer);
                var generator = new Generator(descriptor);
                generator.Generate();

                var content = File.ReadAllText(generator.Config.ExportPath);

                var needToContain = new List<string>
                {
                    "ApiTestService",
                    "ApiSimpleService",
                    "ApiConfigVoidTestService",
                    "ApiGeneratedMethodTestService",
                    "ApiTypeTestService"
                };

                foreach (var controller in needToContain)
                {
                    Assert.IsTrue(content.Contains(controller), "Generator doesnt include registered controller: " + controller);
                }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(GeneratorConfigurationException))]
        public void BaseUrlDoesntEndWithSlash()
        {
            var path = "generate/here/the/code";

            var config = new GeneratorConfig
            {
                ExportPath = path,
                DefaultBaseUrl = "abc"
            };

            var generator = new Generator(null)
            {
                Config = config
            };

            generator.Generate();
        }

        [TestMethod]
        [ExpectedException(typeof(GeneratorConfigurationException))]
        public void UrlPrefixDoesntEndWithSlash()
        {
            var path = "generate/here/the/code";

            var config = new GeneratorConfig
            {
                ExportPath = path,
                DefaultBaseUrl = "abc/",
                UrlSuffix = "efg"
            };

            var generator = new Generator(null)
            {
                Config = config
            };

            generator.Generate();
        }
    }
}
