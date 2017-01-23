using System;
using System.Web.Http;

namespace AngularClientGeneratorTest.TestControllers
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
