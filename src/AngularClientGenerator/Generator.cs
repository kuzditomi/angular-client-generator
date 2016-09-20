using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace AngularClientGenerator
{
    public class Generator : IGeneratorConfig
    {
        private DescriptionCollector DescriptionCollector;

        public Generator(IApiExplorer explorer)
        {
            ExportPath = "angular-generated-client.ts";
            Language = Language.TypeScript;
            
            this.DescriptionCollector = new DescriptionCollector(explorer);
        }

        public string ExportPath { get; set; }

        public Language Language { get; set; }

        public void Generate()
        {
            var controllerDescription = DescriptionCollector.GetControllerDescriptions();

            var names = controllerDescription.Select(c => c.Name);
            var content = String.Concat(names);

            File.WriteAllText(this.ExportPath, content);
        }
    }
}
