using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Core.Descriptor;
using AngularClientGenerator.Core.ExampleWebAPI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.IO;

namespace AngularClientGenerator.Core.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

            var descriptionService = host.Services.GetService(typeof(IApiDescriptionGroupCollectionProvider)) as IApiDescriptionGroupCollectionProvider;
            var filename = "generated.ts";
            var currentProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var destinationPath = Path.Combine(currentProjectPath, @"..\..\AngularClientGenerator.Core.ExampleWebAPI\wwwroot\", filename);

            var descriptor = ApiDescriptorConverter.CreateApiDescriptor(descriptionService);

            var generator = new Generator(descriptor)
            {
                Config = new GeneratorConfig
                {
                    ModuleName = "example-generated",
                    ExportPath = destinationPath,
                    IndentType = IndentType.FourSpace,
                    Language = Language.TypeScript,
                    UseNamespaces = true,
                    UrlSuffix = "",
                    NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.Core.", "")
                }
            };

            generator.Generate();
        }
    }
}
