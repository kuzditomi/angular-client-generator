using System.Collections.Generic;
using System.Web.Http;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/configbiginterrortest")]
    public class DictionaryKeyTypeMismatchTestController : ApiController
    {
        [HttpPost]
        [Route("dictionarywithboolkey")]
        public void DictionaryWithNumberKey(Dictionary<bool, string> dictionary)
        {
        }
    }
}
