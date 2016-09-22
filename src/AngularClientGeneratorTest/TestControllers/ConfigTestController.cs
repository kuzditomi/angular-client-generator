using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AngularClientGeneratorTest.TestModels;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/configtest")]
    public class ConfigTestController: ApiController
    {
        [Route("void")]
        [HttpGet]
        public void VoidParameterlessGetAction()
        {
        }

        [Route("voidstring")]
        [HttpGet]
        public void VoidStringParameterAction(string stringparam)
        {
        }
    }
}
