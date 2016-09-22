using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class ActionDescriptionPart: IDescriptionPart
    {
        public string Name { get; set; }
        public IEnumerable<ParameterDescription> ParameterDescriptions { get; set; }
        public TypeDescriptionPart ReturnValueDescription { get; set; }

        public ActionDescriptionPart(HttpActionDescriptor actionDescriptor)
        {
            var descriptor = actionDescriptor as ReflectedHttpActionDescriptor;

            if (descriptor == null)
                return;

            this.Name = descriptor.ActionName;
            this.ReturnValueDescription = new TypeDescriptionPart(descriptor.MethodInfo.ReturnType);

            this.ParameterDescriptions = actionDescriptor.GetParameters()
                .Select(p => new ParameterDescription(p)).ToList();
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
