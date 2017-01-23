using System.Collections.Generic;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class ModuleDescriptionPart : IDescriptionPart
    {
        public string Name { get; set; }

        public IEnumerable<ControllerDescriptionPart> ControllerDescriptionParts { get; set; }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
