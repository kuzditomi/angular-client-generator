using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AngularClientGenerator.Descriptor.Test.TestModels;
using AngularClientGenerator.Descriptor.Test.TestModels.EnumNameSpace;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
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
