using System.Web.Http;
using AngularClientGenerator.Descriptor.Test.TestModels;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
{
    [RoutePrefix("api/test")]
    public class TestController: ApiController
    {
        [HttpGet]
        [Route("test")]
        public string TestAction()
        {
            return "ok";
        }

        [Route("test2")]
        public int TestAction2()
        {
            return 2;
        }

        [Route("testa")]
        public TestModelA TestActionA()
        {
            return new TestModelA();
        }
        
        [Route("testb")]
        public TestModelB TestActionB()
        {
            return new TestModelB();
        }
    }
}
