using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AngularClientGeneratorTest.TestModels;
using SB.TradingTools.AngularClientGeneratorTest.TestModels;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/methodtest")]
    public class GeneratedMethodTestController : ApiController
    {
        [Route("voidparamless")]
        [HttpGet]
        public void VoidParameterlessGetAction()
        {
        }

        [Route("voidstringparam")]
        [HttpGet]
        public void VoidStringParamGetAction(string stringparameter)
        {
        }

        [Route("voidcomplexparam")]
        [HttpGet]
        public void VoidComplexparamAction(MyEmptyTestClass complex)
        {
        }

        [Route("stringret")]
        [HttpGet]
        public string StringReturnAction()
        {
            return "ok";
        }

        [Route("resptyperet")]
        [HttpGet]
        [ResponseType(typeof(MyEmptyTestClass))]
        public IHttpActionResult ResponseTypeReturnAction()
        {
            return Ok();
        }

        [Route("arrayret")]
        [HttpGet]
        [ResponseType(typeof(MyEmptyTestClass[]))]
        public IHttpActionResult ArrayReturnAction()
        {
            return Ok();
        }

        [Route("enumerableret")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<MyEmptyTestClass>))]
        public IHttpActionResult IEnumerableReturnAction()
        {
            return Ok();
        }
    }
}
