using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class TypeDescriptionPart : IDescriptionPart
    {
        public string TypeName { get; set; }

        public TypeDescriptionPart(Type type)
        {
            TypeName = type.Name;
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
