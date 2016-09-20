using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public class TsApiVisitor : IApiVisitor
    {
        public void Visit(ControllerDescriptionPart controllerDescription)
        {
            throw new NotImplementedException();
        }

        public void Visit(ActionDescriptionPart actionDescription)
        {
            throw new NotImplementedException();
        }
    }
}
