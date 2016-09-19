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
        private IApiExplorer ApiExplorer;

        public Generator(IApiExplorer explorer)
        {
            this.ApiExplorer = explorer;

            ExportPath = "angular-generated-client.ts";
            Language = Language.TypeScript;
        }

        public string ExportPath { get; set; }

        public Language Language { get; set; }

        public void Generate()
        {
            var controllers = this.ApiExplorer.ApiDescriptions
                .Select(d => d.ActionDescriptor.ControllerDescriptor)
                .Distinct();

            var names = controllers.Select(c => c.ControllerName);
            var content = String.Concat(names);

            File.WriteAllText(this.ExportPath, content);
        }
    }
}
