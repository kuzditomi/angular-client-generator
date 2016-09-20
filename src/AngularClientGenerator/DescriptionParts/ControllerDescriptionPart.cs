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
        public IEnumerable<ActionDescriptionPart> ActionDescriptions { get; private set; }

        public ControllerDescriptionPart(HttpControllerDescriptor controllerDescriptor, IEnumerable<HttpActionDescriptor> actionDescriptions)
        {
            this.Name = controllerDescriptor.ControllerName;
            this.ActionDescriptions = actionDescriptions
                .Select(a => new ActionDescriptionPart(a));
        }

        public string Name { get; set; }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);

            foreach (var actionDescriptionPart in this.ActionDescriptions)
            {
                actionDescriptionPart.Accept(visitor);
            }
        }
    }
}
