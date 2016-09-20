using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public interface IDescriptionPart
    {
        void Accept(IApiVisitor visitor);
    }
}
