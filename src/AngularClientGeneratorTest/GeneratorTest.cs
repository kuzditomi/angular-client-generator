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
using AngularClientGeneratorTest.Util;
using Microsoft.Owin.Hosting;
using Owin;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class GeneratorTest
    {
        private HttpConfiguration HttpConfiguration { get; set; }
        private IApiExplorer ApiExplorer { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CustomHttpControllerTypeResolver.ClearTypesToDiscover();
        }

        [TestMethod]
        public void CreateGeneratorDefaultParams()
        {
            var generator = new Generator(this.ApiExplorer);

            Assert.AreEqual("angular-generated-client.ts", generator.ExportPath);
            Assert.AreEqual(Language.TypeScript, generator.Language);
        }

        [TestMethod]
        public void CreateGeneratorOverriddenParams()
        {
            var path = "generate/here/the/code";

            var generator = new Generator(this.ApiExplorer)
            {
                ExportPath = path
            };

            Assert.AreEqual(path, generator.ExportPath);
        }

        [TestMethod]
        public void GenerateCode()
        {
            this.HttpConfiguration = new HttpConfiguration();
            this.ApiExplorer = new ApiExplorer(this.HttpConfiguration);

            var generator = new Generator(this.ApiExplorer);
            generator.Generate();

            var fileExists = File.Exists(generator.ExportPath);

            Assert.IsTrue(fileExists);
        }
        
        [TestMethod]
        public void GenerateAllRegisteredControllers()
        {
            CustomHttpControllerTypeResolver.RegisterTypeToDiscover(typeof(TestController));
            CustomHttpControllerTypeResolver.RegisterTypeToDiscover(typeof(SimpleController));

            this.RunInScope(() =>
            {
                var generator = new Generator(this.ApiExplorer);
                generator.Generate();

                var content = File.ReadAllText(generator.ExportPath);
                var containsTestControllerDefinition = content.Contains("Test");
                var containsSimpleControllerDefinition = content.Contains("Simple");

                Assert.IsTrue(containsTestControllerDefinition, "Generator doesnt include registered TestController");
                Assert.IsTrue(containsSimpleControllerDefinition, "Generator doesnt include registered SimpleController");
            });
        }

        private void RunInScope(Action action)
        {
            string baseAddress = "http://localhost:9874/bar";
            using (var server = WebApp.Start(url: baseAddress, startup: app =>
            {
                this.HttpConfiguration = new HttpConfiguration();
                this.HttpConfiguration.Services.Replace(typeof(IHttpControllerTypeResolver), new CustomHttpControllerTypeResolver());
                this.HttpConfiguration.MapHttpAttributeRoutes();
                this.HttpConfiguration.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                app.UseWebApi(this.HttpConfiguration);
                this.HttpConfiguration.EnsureInitialized();
                this.ApiExplorer = this.HttpConfiguration.Services.GetApiExplorer();
            }))
            {
                action();
            };
        }
    }
}
