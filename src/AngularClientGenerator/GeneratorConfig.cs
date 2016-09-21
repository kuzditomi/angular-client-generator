using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator
{
    public class GeneratorConfig
    {
        public string ExportPath { get; set; }
        public Language Language { get; set; }
        public string ModuleName { get; set; }
        public IndentType IndentType { get; set; }

        public GeneratorConfig()
        {
            this.ExportPath = "angular-generated-client.ts";
            this.Language = Language.TypeScript;
            this.ModuleName = "mymodule";
            this.IndentType = IndentType.Tab;
        }
    }
}
