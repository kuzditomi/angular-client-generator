using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularClientGenerator.Descriptor.Test.TestModels
{
    public class ComplexDeleteType
    {
        public string CategoryId { get; set; }
        public string ProfileName { get; set; }

        public IEnumerable<InnerComplexDeleteType> ComplexType { get; set; }
    }

    public class InnerComplexDeleteType
    {
        public string StringProperty { get; set; }
    }
}
