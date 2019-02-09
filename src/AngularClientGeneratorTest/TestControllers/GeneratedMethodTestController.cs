using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AngularClientGeneratorTest.TestModels;

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

        [Route("generictyperet")]
        [HttpGet]
        [ResponseType(typeof(GenericTypeClass<int>))]
        public IHttpActionResult GenericTypeReturnAction()
        {
            return Ok();
        }

        [Route("deletedata")]
        [HttpDelete]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteComplexParam(ComplexDeleteType param)
        {
            return Ok();
        }


        [Route("deletesimpledata")]
        [HttpDelete]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteSimpleParam(string param)
        {
            return Ok();
        }

        [Route("deletesimpleenumerabledata")]
        [HttpDelete]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteSimpleEnumerableParam(IEnumerable<int> param)
        {
            return Ok();
        }

        [Route("deleteurlreplaceandbody/{id}")]
        [HttpDelete]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteUrlReplaceAndBody(int id, ComplexDeleteType param)
        {
            return Ok();
        }
    }
}
