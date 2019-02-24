using System.Collections.Generic;

namespace AngularClientGenerator.Test.TestModels
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
