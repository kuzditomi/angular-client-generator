﻿using System.IO;
using System.Web.Http;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
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
                string destinationPath;
                if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
                {
                    destinationPath = args[0];
                }
                else
                {
                    var filename = "generated.ts";
                    var currentProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    destinationPath = Path.Combine(currentProjectPath, @"..\AngularClientGenerator.ExampleWebAPI\app\", filename);
                }

                var explorer = config.Services.GetApiExplorer();
                var generator = new Generator(explorer)
                {
                    Config = new GeneratorConfig
                    {
                        ModuleName = "example-generated",
                        ExportPath = destinationPath,
                        IndentType = IndentType.FourSpace,
                        Language = Language.TypeScript,
                        UseNamespaces = true,
                        NamespaceNamingRule = type => type.Namespace.Replace("AngularClientGenerator.", "")
                    }
                };

                generator.Generate();
            }
        }
    }
}