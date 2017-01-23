using System.Collections.Generic;
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
        public void SameNameDifferentNameSpaceA(SB.TradingTools.AngularClientGeneratorTest.TestModels.NameSpaceA.SameNameDifferentNameSpace parameter)
        {
        }

        [HttpPost]
        [Route("samenameb")]
        public void SameNameDifferentNameSpaceB(SB.TradingTools.AngularClientGeneratorTest.TestModels.NameSpaceB.SameNameDifferentNameSpace parameter)
        {
        }

        [HttpPost]
        [Route("array")]
        public void NameSpacedArrays(IEnumerable<SB.TradingTools.AngularClientGeneratorTest.TestModels.NameSpaceA.SameNameDifferentNameSpace> parameter)
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
