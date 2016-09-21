using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public class TsApiVisitor : ApiVisitor
    {
        public TsApiVisitor(GeneratorConfig config) :base(config)
        {
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.ClientBuilder.Write(controllerDescription.Name);   
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
        }
    }
}
