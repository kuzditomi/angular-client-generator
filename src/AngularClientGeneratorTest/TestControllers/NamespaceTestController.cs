using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AngularClientGeneratorTest.TestModels;
using AngularClientGeneratorTest.TestModels.EnumNameSpace;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("namespaces")]
    public class NamespaceTestController: ApiController
    {
        [HttpPost]
        [Route("samenamea")]
        public void SameNameDifferentNameSpaceA(TestModels.NameSpaceA.SameNameDifferentNameSpace parameter)
        {
        }

        [HttpPost]
        [Route("samenameb")]
        public void SameNameDifferentNameSpaceB(TestModels.NameSpaceB.SameNameDifferentNameSpace parameter)
        {
        }

        [HttpPost]
        [Route("array")]
        public void NameSpacedArrays(IEnumerable<TestModels.NameSpaceA.SameNameDifferentNameSpace> parameter)
        {
        }
        
        [HttpPost]
        [Route("namespacedproperty")]
        public void NamespacedProperties(HasNameSpacedProperties parameter)
        {
        }

        [HttpGet]
        [Route("enum")]
        public void NamespacedEnum(OnlyEnumInNameSpace enumparam)
        {
        }
    }
}
