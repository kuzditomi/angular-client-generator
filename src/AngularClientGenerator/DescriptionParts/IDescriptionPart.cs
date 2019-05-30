using AngularClientGenerator.Visitors;

namespace AngularClientGenerator.DescriptionParts
{
    public interface IDescriptionPart
    {
        void Accept(IApiVisitor visitor);
    }
}
