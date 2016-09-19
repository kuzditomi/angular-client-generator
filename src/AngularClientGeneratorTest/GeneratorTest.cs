using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using System.Web.Http.Description;
using System.Web.Http;
using System.IO;
using AngularClientGeneratorTest.TestControllers;
using System.Web.Http.Dependencies;
using Unity.WebApi;
using Microsoft.Practices.Unity;
using Microsoft.Owin.Hosting;
using AngularClientGeneratorTest.TestStartups;
using Owin;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class GeneratorTest
    {
        private HttpConfiguration HttpConfiguration { get; set; }
        private IApiExplorer ApiExplorer { get; set; }
        private IDependencyResolver DependencyResolver { get; set; }
        private IUnityContainer Container { get; set; }

        [TestInitialize]
        public void Startup()
        {
            this.HttpConfiguration = new HttpConfiguration();
            this.Container = new UnityContainer();
            this.DependencyResolver = new UnityDependencyResolver(this.Container);
            this.HttpConfiguration.DependencyResolver = this.DependencyResolver;
            this.ApiExplorer = new ApiExplorer(this.HttpConfiguration);
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
            var generator = new Generator(this.ApiExplorer);
            generator.Generate();

            var fileExists = File.Exists(generator.ExportPath);

            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public void GenerateEmptyController()
        {
            this.Container.RegisterType<EmptyController>();
            this.RegisterResolver();

            this.RunInScope(() =>
            {
                var controllerName = "Empty";

                var generator = new Generator(this.ApiExplorer);
                generator.Generate();

                var content = File.ReadAllText(generator.ExportPath);
                var containsControllerDefinition = content.Contains(controllerName);

                Assert.IsTrue(containsControllerDefinition, "Generator doesnt include registered controllers");
            });
        }

        [TestMethod]
        public void GenerateRegisteredControllers()
        {
            this.Container.RegisterType<EmptyController>();
            this.RegisterResolver();

            this.RunInScope(() =>
            {
                var controllerName = "Test";

                var generator = new Generator(this.ApiExplorer);
                generator.Generate();

                var content = File.ReadAllText(generator.ExportPath);
                var containsControllerDefinition = content.Contains(controllerName);

                Assert.IsFalse(containsControllerDefinition, "Generator adds more controller than registered");
            });
        }

        private void RegisterResolver()
        {
            this.DependencyResolver = new UnityDependencyResolver(this.Container);
            this.HttpConfiguration.DependencyResolver = this.DependencyResolver;
            this.HttpConfiguration.MapHttpAttributeRoutes();
            this.HttpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private void RunInScope(Action action)
        {
            string baseAddress = "http://localhost:9874/bar";
            using (var server = WebApp.Start(url: baseAddress, startup: app =>
            {
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
