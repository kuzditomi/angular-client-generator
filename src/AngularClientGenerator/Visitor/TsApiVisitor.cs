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
        private Dictionary<string, TypeDescriptionPart> Types { get; }

        public TsApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
            this.Types = new Dictionary<string, TypeDescriptionPart>();
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            this.ClientBuilder.WriteLine("export class Api{0}Service {{", controllerDescription.Name);
            this.ClientBuilder.IncreaseIndent();
            this.ClientBuilder.WriteLine("static $inject = ['$http', '$q'];", controllerDescription.Name);
            this.ClientBuilder.WriteLine("constructor(private http, private q){{ }}", controllerDescription.Name);

            foreach (var actionDescriptionPart in controllerDescription.ActionDescriptionParts)
            {
                actionDescriptionPart.Accept(this);
            }

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}", controllerDescription.Name);
            this.ClientBuilder.WriteLine();
            this.ClientBuilder.WriteLine("angular.module('{0}').factory(Api{1}Service);", Config.ModuleName, controllerDescription.Name);
            this.ClientBuilder.WriteLine();
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
            // collect return value type for later
            actionDescription.ReturnValueDescription.Accept(this);

            // collect parameter types for later
            foreach (var actionDescriptionParameterDescription in actionDescription.ParameterDescriptions)
            {
                actionDescriptionParameterDescription.Accept(this);
            }

            GenerateConfigFor(actionDescription);
            GenerateMethodFor(actionDescription);
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            foreach (var controllerDescriptionPart in moduleDescription.ControllerDescriptionParts)
            {
                controllerDescriptionPart.Accept(this);
            }

            this.WriteTypes();
        }

        public override void Visit(TypeDescriptionPart typeDescriptionPart)
        {
            if (!this.Types.ContainsKey(typeDescriptionPart.TypeName))
                this.Types.Add(typeDescriptionPart.TypeName, typeDescriptionPart);
        }

        private void GenerateMethodFor(ActionDescriptionPart actionDescription)
        {
            // method header
            this.ClientBuilder.WriteLine("public {0}() : ng.IPromise<{1}> {{", actionDescription.Name, actionDescription.ReturnValueDescription.TypeName);
            // method footer
            this.ClientBuilder.WriteLine("}}");
        }

        private void GenerateConfigFor(ActionDescriptionPart actionDescription)
        {
            // method header
            this.ClientBuilder.WriteLine("public {0}Config() : ng.IRequestConfig {{", actionDescription.Name, actionDescription.ReturnValueDescription.TypeName);
            this.ClientBuilder.IncreaseIndent();

            // method body
            this.ClientBuilder.WriteLine("return {{");
            this.ClientBuilder.IncreaseIndent();

            this.ClientBuilder.WriteLine("url: '{0}',", actionDescription.UrlTemplate);
            this.ClientBuilder.WriteLine("method: '{0}'", actionDescription.HttpMethod.ToString().ToUpper());

            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}};");

            // method footer
            this.ClientBuilder.DecreaseIndent();
            this.ClientBuilder.WriteLine("}}");
        }

        private void WriteTypes()
        {
            foreach (var type in Types)
            {
                this.WriteType(type.Value);
            }
        }

        private void WriteType(TypeDescriptionPart type)
        {
            this.ClientBuilder.WriteLine("export interface {0} {{ }}", type.TypeName);
        }
    }
}
