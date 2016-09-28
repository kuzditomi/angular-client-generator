using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
