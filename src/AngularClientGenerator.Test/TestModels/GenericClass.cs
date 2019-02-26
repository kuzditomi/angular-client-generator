using System.Collections.Generic;

namespace AngularClientGenerator.Test.TestModels
{
    public class GenericClass<T>
    {
        public IEnumerable<T> GenericList { get; set; }
        public T GenericProperty { get; set; }
    }
}
