using System.Collections.Generic;
using AngularClientGenerator.Visitors;

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
