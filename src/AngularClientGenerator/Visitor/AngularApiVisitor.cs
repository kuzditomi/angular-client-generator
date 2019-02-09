using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public class AngularApiVisitor : AngularJSTypescriptApiVisitor
    {
        protected AngularApiVisitor(IVisitorConfig config, ClientBuilder builder) : base(config, builder)
        {
        }

        public override void Visit(ControllerDescriptionPart controllerDescription)
        {
            throw new System.NotImplementedException();
        }

        public override void Visit(ActionDescriptionPart actionDescription)
        {
            throw new System.NotImplementedException();
        }

        public override void Visit(ModuleDescriptionPart moduleDescription)
        {
            throw new System.NotImplementedException();
        }

        public override void Visit(TypeDescriptionPart typeDescriptionPart)
        {
            throw new System.NotImplementedException();
        }
    }
}
