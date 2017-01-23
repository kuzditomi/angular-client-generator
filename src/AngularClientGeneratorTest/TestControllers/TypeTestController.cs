using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AngularClientGeneratorTest.TestModels;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/type")]
    public class TypeTestController: ApiController
    {
        [HttpGet]
        [Route("basictypes")]
        public void BasicTypesAction(string s, int a, double b, float c, decimal d, bool f)
        {
        }

        [HttpGet]
        [Route("arrayvalue")]
        public void ArrayTypeAction(string[] arr)
        {

        }

        [HttpGet]
        [Route("enumtype")]
        public void EnumTypeAction(TestEnum enumvalue)
        {

        }
        
        [HttpGet]
        [Route("numberenumtype")]
        public void NumberedEnumTypeAction(TestNumberedEnum enumvalue)
        {

        }

        [HttpGet]
        [Route("doubletype1")]
        public void OneMyEmptyTestClassAction(MyEmptyTestClass model)
        {

        }

        [HttpGet]
        [Route("doubletype2")]
        public void TwoMyEmptyTestClassAction(MyEmptyTestClass model)
        {

        }

        [HttpGet]
        [Route("recursivediscover")]
        public void TestRecursiveDiscovery(TestComplexType model)
        {

        }

        [HttpPost]
        [Route("arrayonly")]
        public void ArrayTypesAction(ArrayOnlyType[] model)
        {
        }

        [HttpPost]
        [Route("enumerablevalue")]
        public void EnumerableTypeAction(IEnumerable<EnumerableOnlyType> model)
        {

        }

        [HttpPost]
        [Route("optionalparam")]
        public void OptionalParam(int? optional = 3)
        {

        }

        [HttpPost]
        [Route("nullableprop")]
        public void NullableProperty(ContainsNullableProperty hasnullable)
        {

        }

        [HttpPost]
        [Route("asoptionalparamonly")]
        public void OnlyAsOptionalParam(AsOptionalParamOnly model = null)
        {

        }

        [HttpPost]
        [Route("actionresult")]
        public IHttpActionResult ActionResultWithoutAttribute()
        {
            return Ok();
        }

        [HttpPost]
        [Route("unwraptask")]
        public async Task<IHttpActionResult> UnWrapTaskGeneric()
        {
            return Ok();
        }

        [HttpPost]
        [Route("tasktovoid")]
        public async Task TaskToVoid()
        {
        }

        [HttpPost]
        [Route("datetimereplace")]
        public void DateTimeReplaced(DateTime date)
        {
        }

        [HttpPost]
        [Route("dictionarywithstringkey")]
        public void DictionaryWithStringKey(Dictionary<string, string> dictionary)
        {
        }

        [HttpPost]
        [Route("dictionarywithnumberkey")]
        public void DictionaryWithNumberKey(Dictionary<int, string> dictionary)
        {
        }

        [HttpPost]
        [Route("dictionarywithcomplexvalue")]
        public void DictionaryWithComplexValue(Dictionary<int, DictionaryReturnType> dictionary)
        {
        }
    }
}
