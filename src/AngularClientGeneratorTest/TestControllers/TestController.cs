using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AngularClientGeneratorTest.TestModels;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/test")]
    public class TestController: ApiController
    {
        public TestController()
        {
            
        }

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
