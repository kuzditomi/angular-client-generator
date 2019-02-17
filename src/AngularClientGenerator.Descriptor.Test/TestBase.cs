using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Descriptor.Test.Util;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using Owin;

namespace AngularClientGenerator.Descriptor.Test
{
    [TestFixture]
    public abstract class TestBase
    {
        protected HttpConfiguration HttpConfiguration { get; set; }
        protected IApiExplorer ApiExplorer { get; set; }
        protected ApiDescriptor ApiDescriptor { get; set; }

        [SetUp]
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
                this.ApiDescriptor = ApiDescriptorConverter.CreateApiDescriptor(this.ApiExplorer);
            }))
            {
                action();
            }
        }
    }
}
