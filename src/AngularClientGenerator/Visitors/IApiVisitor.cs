using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitors
{
    public interface IApiVisitor
    {
        void Visit(ControllerDescriptionPart controllerDescription);
        void Visit(ActionDescriptionPart actionDescription);
        void Visit(ModuleDescriptionPart moduleDescription);
        void Visit(TypeDescriptionPart typeDescriptionPart);
        string GetContent();
    }
}
