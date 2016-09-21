using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class ControllerDescriptionPart : IDescriptionPart
    {
        public string Name { get; set; }
        public IEnumerable<ActionDescriptionPart> ActionDescriptionParts { get; set; }

        public ControllerDescriptionPart(HttpControllerDescriptor controllerDescriptor)
        {
            this.Name = controllerDescriptor.ControllerName;
        }
        
        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
