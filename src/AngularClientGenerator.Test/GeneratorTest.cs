using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Contracts.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AngularClientGenerator.Test
{
    [TestClass]
    public class GeneratorTest
    {
        [TestMethod]
        public void CreateGeneratorDefaultParams()
        {
            var generator = new Generator(null);

            Assert.AreEqual("angular-generated-client.ts", generator.Config.ExportPath);
            Assert.AreEqual(ClientType.AngularJsTypeScript, generator.Config.Language);
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
            var descriptor = new ApiDescriptor
            {
                ControllerDescriptors = Enumerable.Empty<ControllerDescriptor>()
            };

            var generator = new Generator(descriptor);
            generator.Generate();

            var fileExists = File.Exists(generator.Config.ExportPath);

            Assert.IsTrue(fileExists);
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

        [TestMethod]
        public void GenerateAllRegisteredControllers()
        {
            var controllerNames = new[] { "A", "B", "C" };
            var descriptor = new ApiDescriptor
            {
                ControllerDescriptors = controllerNames.Select(name => new ControllerDescriptor
                {
                    Name = name,
                    ActionDescriptors = Enumerable.Empty<ActionDescriptor>()
                })
            };

            var generator = new Generator(descriptor);
            generator.Generate();

            var content = File.ReadAllText(generator.Config.ExportPath);

            var needToContain = new List<string> {
                "ApiAService",
                "ApiBService",
                "ApiCService",
            };

            foreach (var controller in needToContain)
            {
                Assert.IsTrue(content.Contains(controller), "Generator doesnt include registered controller: " + controller);
            }
        }
    }
}
