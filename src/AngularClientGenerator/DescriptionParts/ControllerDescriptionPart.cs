using System.Collections.Generic;
using System.Linq;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class ControllerDescriptionPart : IDescriptionPart
    {
        public string Name { get; set; }
        public IEnumerable<ActionDescriptionPart> ActionDescriptionParts { get; set; }

        public ControllerDescriptionPart(ControllerDescriptor controllerDescriptor)
        {
            this.Name = controllerDescriptor.Name;
            this.ActionDescriptionParts = controllerDescriptor.ActionDescriptors.Select(ad => new ActionDescriptionPart(ad));
        }
        
        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
