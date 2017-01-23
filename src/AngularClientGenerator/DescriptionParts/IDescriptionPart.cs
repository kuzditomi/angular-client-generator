using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public interface IDescriptionPart
    {
        void Accept(IApiVisitor visitor);
    }
}
