using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularClientGeneratorTest.TestModels
{
    public class TestComplexType
    {
        public string NormalProperty { get; set; }
        public TestComplexInnerProperty ComplexProperty { get; set; }
        public int NumberProperty { get; set; }
        public TestEnum Enum { get; set; }
    }

    public class TestComplexInnerProperty
    {
        public int SomeNumber { get; set; }
    }
}
