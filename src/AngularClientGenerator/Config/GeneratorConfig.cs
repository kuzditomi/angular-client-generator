using System;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Config;

namespace AngularClientGenerator.Config
{
    public class GeneratorConfig : IGeneratorConfig, IClientBuilderConfig, IVisitorConfig
    {
        public string ExportPath { get; set; }
        public Language Language { get; set; }
        public string ModuleName { get; set; }
        public IndentType IndentType { get; set; }
        public bool UseNamespaces { get; set; }
        public string DefaultBaseUrl { get; set; }
        public string UrlSuffix { get; set; }
        public Func<Type, string> NamespaceNamingRule { get; set; }

        public GeneratorConfig()
        {
            this.ExportPath = "angular-generated-client.ts";
            this.Language = Language.TypeScript;
            this.ModuleName = "mymodule";
            this.IndentType = IndentType.Tab;
            this.UseNamespaces = false;
            this.DefaultBaseUrl = "http://localhost:1337/";
            this.UrlSuffix = "";
        }
    }
}
