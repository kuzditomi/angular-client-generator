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
        public TsApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.ClientBuilder.WriteLine("export class Api{0}Service {{", controllerDescription.Name);
            this.ClientBuilder.WriteLine("}}", controllerDescription.Name);
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("angular.module('{0}').factory(Api{1}Service);", Config.ModuleName, controllerDescription.Name);
            this.ClientBuilder.WriteLine();
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
        }
    }
}
