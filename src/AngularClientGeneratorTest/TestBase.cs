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
            };
        }
    }
}
