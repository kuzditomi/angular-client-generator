using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Practices.Unity;
using AngularClientGeneratorTest.TestControllers;
using Unity.WebApi;
using System.Web.Http;

[assembly: OwinStartup(typeof(AngularClientGeneratorTest.TestStartups.TestStartup))]

namespace AngularClientGeneratorTest.TestStartups
{
    public class TestStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new UnityContainer();
            container.RegisterType<EmptyController>();

            var dependencyResolver = new UnityDependencyResolver(container);
            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.DependencyResolver = dependencyResolver;

            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            httpConfiguration.EnsureInitialized();
            
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
