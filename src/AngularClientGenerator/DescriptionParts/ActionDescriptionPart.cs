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
        public TypeDescription ReturnValueDescription { get; set; }

        public ActionDescriptionPart(HttpActionDescriptor actionDescriptor)
        {
            this.Name = actionDescriptor.ActionName;
            this.ReturnValueDescription = new TypeDescription
            {
                TypeName = actionDescriptor.ReturnType.Name
            };
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
