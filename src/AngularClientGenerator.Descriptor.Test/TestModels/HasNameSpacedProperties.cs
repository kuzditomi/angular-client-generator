using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularClientGenerator.Descriptor.Test.TestModels
{
    public class HasNameSpacedProperties
    {
        public NameSpaceA.SameNameDifferentNameSpace AProperty { get; set; }
        public NameSpaceB.SameNameDifferentNameSpace BProperty { get; set; }
    }
}
