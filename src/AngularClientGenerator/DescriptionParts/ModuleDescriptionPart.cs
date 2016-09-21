using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class ModuleDescriptionPart : IDescriptionPart
    {
        public string Name { get; set; }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
