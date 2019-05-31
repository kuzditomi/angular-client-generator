using System.IO;
using System.Web.Http;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Descriptor;
using AngularClientGenerator.ExampleWebAPI;
using Microsoft.Owin.Hosting;

namespace AngularClientGenerator.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:11223";
            var config = new HttpConfiguration();

            using (WebApp.Start(url, app =>
            {
                WebApiConfig.Register(config);
                config.EnsureInitialized();
            }))
            {
                var filename = "generated.ts";
                var currentProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                var angularJsDestinationPath = Path.Combine(currentProjectPath, @"..\AngularClientGenerator.ExampleWebAPI\angularjs-example\app\", filename);
                var angularDestinationPath = Path.Combine(currentProjectPath, @"..\AngularClientGenerator.ExampleWebAPI\angular-example\src\app\", filename);

                var explorer = config.Services.GetApiExplorer();
                var descriptor = ApiDescriptorConverter.CreateApiDescriptor(explorer);

                // Generate angularjs
                var angularJSGenerator = new Generator(descriptor)
                {
                    Config = new GeneratorConfig
                    {
                        ModuleName = "example-generated",
                        ExportPath = angularJsDestinationPath,
                        IndentType = IndentType.FourSpace,
                        ClientType = ClientType.AngularJsTypeScript,
                        UseNamespaces = true,
                        UrlSuffix = "",
                        NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.", "")
                    }
                };
                angularJSGenerator.Generate();

                // Generate angular
                var angularGenerator = new Generator(descriptor)
                {
                    Config = new GeneratorConfig
                    {
                        ModuleName = "ExampleGeneratedModule",
                        ExportPath = angularDestinationPath,
                        IndentType = IndentType.FourSpace,
                        ClientType = ClientType.Angular,
                        UseNamespaces = true,
                        UrlSuffix = "",
                        NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.", "")
                    }
                };
                angularGenerator.Generate();
            }
        }
    }
}
