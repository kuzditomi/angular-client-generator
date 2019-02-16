using System.Web.Http;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
{
    [RoutePrefix("api/simple")]
    public class SimpleController: ApiController
    {
        [Route("one")]
        public IHttpActionResult ActionOne()
        {
            return Ok();
        }

        [Route("two")]
        public IHttpActionResult ActionTwo(int a, string b)
        {
            return Ok();
        }

        [HttpGet]
        [Route("three")]
        public IHttpActionResult ActionThree(int? a = 3)
        {
            return Ok();
        }
    }
}
