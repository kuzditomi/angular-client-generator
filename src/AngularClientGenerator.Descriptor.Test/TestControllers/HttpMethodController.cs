using System.Web.Http;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
{
    [RoutePrefix("api/httpmethod")]
    public class HttpMethodController : ApiController
    {
        [HttpGet]
        [Route("get")]
        public void TestGET()
        {
        }

        [HttpPost]
        [Route("post")]
        public void TestPOST()
        {
        }

        [HttpDelete]
        [Route("delete")]
        public void TestDELETE()
        {
        }

        [HttpPut]
        [Route("put")]
        public void TestPUT()
        {
        }
    }
}
