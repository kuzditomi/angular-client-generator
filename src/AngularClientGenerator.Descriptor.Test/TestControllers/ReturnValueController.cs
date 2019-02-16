using System.Web.Http;
using System.Web.Http.Description;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
{
    [RoutePrefix("api/returnvalue")]
    public class ReturnValueController : ApiController
    {
        [Route("void")]
        public void TestVoid()
        {
        }

        [Route("int")]
        public int TestInt()
        {
            return 2;
        }

        [Route("result")]
        public IHttpActionResult TestResult()
        {
            return Ok();
        }

        [Route("response")]
        [ResponseType(typeof(int))]
        public IHttpActionResult TestResponseType()
        {
            return Ok();
        }
    }
}
