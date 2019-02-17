using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using AngularClientGeneratorTest.Util;
using Microsoft.Owin.Hosting;
using Owin;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Descriptor;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public abstract class TestBase
    {
        protected HttpConfiguration HttpConfiguration { get; set; }
        protected IApiExplorer ApiExplorer { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CustomHttpControllerTypeResolver.ClearTypesToDiscover();
        }

        protected void RegisterController<T>() where T : ApiController
        {
            CustomHttpControllerTypeResolver.RegisterTypeToDiscover(typeof(T));
        }

        protected void RunInScope(Action action)
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
            }
        }

        protected string VisitModuleWithController<T>(GeneratorConfig config = null) where T : ApiController
        {
            if (config == null)
            {
                config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
            }

            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

            var apiDescriptions = ApiExplorer
                .ApiDescriptions
                .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerType == typeof(T));
            var controllerName = apiDescriptions.First().ActionDescriptor.ControllerDescriptor.ControllerName;

            var descriptor = ApiDescriptorConverter.CreateApiDescriptor(ApiExplorer);
            var controllerDescriptor = descriptor.ControllerDescriptors.First(c => c.Name == controllerName);
            var module = new ModuleDescriptionPart
            {
                ControllerDescriptionParts = new ControllerDescriptionPart[]
                {
                    new ControllerDescriptionPart(controllerDescriptor)
                }
            };

            module.Accept(apiVisitor);

            return apiVisitor.GetContent();
        }

        protected string VisitActionsFromController<T>(GeneratorConfig config = null) where T : ApiController
        {
            if (config == null)
            {
                config = new GeneratorConfig
                {
                    IndentType = IndentType.Tab,
                    Language = Language.TypeScript
                };
            }

            var builder = new ClientBuilder(config);
            var apiVisitor = new AngularJSTypescriptApiVisitor(config, builder);

            var descriptor = ApiDescriptorConverter.CreateApiDescriptor(ApiExplorer);
            var controllerName = ApiExplorer
                .ApiDescriptions
                .First(a => a.ActionDescriptor.ControllerDescriptor.ControllerType == typeof(T))
                .ActionDescriptor.ControllerDescriptor.ControllerName;

            var actionDescriptors = descriptor.ControllerDescriptors.First(c => c.Name == controllerName)
                .ActionDescriptors;

            foreach (var actionDescriptor in actionDescriptors)
            {
                var actionDescriptorPart = new ActionDescriptionPart(actionDescriptor);
                actionDescriptorPart.Accept(apiVisitor);
            }

            return apiVisitor.GetContent();
        }
    }
}
