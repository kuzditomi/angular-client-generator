using System;
using System.Web.Http;
using AngularClientGenerator.Descriptor.Test.TestModels;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
{
    [RoutePrefix("api/configbiginterrortest")]
    public class ConfigBigintErrorTestController: ApiController
    {
        [Route("voidbigininterror")]
        [HttpGet]
        public void VoidBigintErrorAction(Int64 bigintparam)
        {
        }
    }
}
