using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public interface IApiVisitor
    {
        void Visit(ControllerDescriptionPart controllerDescription);
        void Visit(ActionDescriptionPart actionDescription);
    }
}
