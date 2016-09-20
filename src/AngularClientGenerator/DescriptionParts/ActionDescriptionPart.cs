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
        public ActionDescriptionPart(HttpActionDescriptor actionDescriptor)
        {
            this.Name = actionDescriptor.ActionName;
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Name { get; set; }
    }
}
