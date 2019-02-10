using System.Web.Http;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
{
    [RoutePrefix("api/simple")]
    public class SimpleController: ApiController
    {
        public SimpleController()
        {

        }

        [Route("one")]
        public IHttpActionResult ActionOne()
        {
            return Ok();
        }

        [Route("two")]
        public IHttpActionResult ActionTwo()
        {
            return Ok();
        }

        [Route("three")]
        public IHttpActionResult ActionThree()
        {
            return Ok();
        }
    }
}
