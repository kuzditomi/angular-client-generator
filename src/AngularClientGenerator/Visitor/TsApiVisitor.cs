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
        private HashSet<string> VisitedControllers;

        public TsApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
            this.VisitedControllers = new HashSet<string>();
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            if (!this.VisitedControllers.Contains(controllerDescription.Name))
            {
                this.ClientBuilder.WriteLine("export class Api{0}Service {{", controllerDescription.Name);

                this.ClientBuilder.IncreaseIndent();
                this.ClientBuilder.WriteLine("static $inject = ['$http', '$q'];", controllerDescription.Name);
                this.ClientBuilder.WriteLine("constructor(private http, private q){{ }}", controllerDescription.Name);
            }
            else
            {
                this.ClientBuilder.DecreaseIndent();
                this.ClientBuilder.WriteLine("}}", controllerDescription.Name);
                this.ClientBuilder.WriteLine();
                this.ClientBuilder.WriteLine("angular.module('{0}').factory(Api{1}Service);", Config.ModuleName, controllerDescription.Name);
                this.ClientBuilder.WriteLine();
            }

            this.VisitedControllers.Add(controllerDescription.Name);
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
            this.ClientBuilder.WriteLine("public {0} () {{", actionDescription.Name);
            this.ClientBuilder.WriteLine("}}");
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
        }
    }
}
