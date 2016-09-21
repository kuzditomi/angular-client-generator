using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;
using AngularClientGenerator.DescriptionParts;
using AngularClientGenerator.Visitor;
using Microsoft.Win32.SafeHandles;

namespace AngularClientGenerator
{
    public class Generator{

        private DescriptionCollector DescriptionCollector;
        private IApiVisitor Visitor;

        public GeneratorConfig Config { get; set; }
        
        public Generator(IApiExplorer explorer)
        {
            this.Config = new GeneratorConfig();
            this.DescriptionCollector = new DescriptionCollector(explorer);
        }
        
        public void Generate()
        {
            switch (this.Config.Language)
            {
                case Language.TypeScript:
                    this.Visitor = new TsApiVisitor(this.Config);
                    break;
            }

            var moduleDescription = new ModuleDescriptionPart()
            {
                Name = this.Config.ModuleName
            };

            var controllerDescriptions = DescriptionCollector.GetControllerDescriptions();

            moduleDescription.Accept(this.Visitor);
            foreach (var controllerDescription in controllerDescriptions)
            {
                controllerDescription.Accept(this.Visitor);
            }

            var content = this.Visitor.GetContent();

            File.WriteAllText(this.Config.ExportPath, content);
        }
    }
}
