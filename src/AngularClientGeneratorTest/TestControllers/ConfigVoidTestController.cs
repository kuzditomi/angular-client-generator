using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AngularClientGeneratorTest.TestModels;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/configtest")]
    public class ConfigVoidTestController: ApiController
    {
        [Route("void")]
        [HttpGet]
        public void VoidParameterlessGetAction()
        {
        }

        [Route("voidstring")]
        [HttpGet]
        public void VoidStringParameterAction(string stringparam)
        {
        }

        [Route("voidint")]
        [HttpGet]
        public void VoidIntParameterAction(int intparam)
        {
        }

        [Route("voiddouble")]
        [HttpGet]
        public void VoidDoubleParameterAction(double doubleparam)
        {
        }

        [Route("voiddecimal")]
        [HttpGet]
        public void VoidDecimalParameterAction(decimal decimalparam)
        {
        }

        [Route("voidsimpleparams")]
        [HttpGet]
        public void VoidSimpleParametersAction(string a, int b, decimal c)
        {
        }

        [Route("voidreplaceparams/{id}")]
        [HttpGet]
        public void VoidReplaceNumberAction(int id)
        {
        }

        [Route("voidreplacemoreparams/{id}/more/{second}")]
        [HttpGet]
        public void VoidReplaceMoreParamsAction(int id, string second)
        {
        }


        [Route("voidcomplexparampost")]
        [HttpPost]
        public void VoidComplexParamPostAction(MyEmptyTestClass complex)
        {
        }

        [Route("voidcomplexparamput")]
        [HttpPut]
        public void VoidComplexParamPutAction(MyEmptyTestClass complex)
        {
        }

        [Route("voidcomplexparamandreplace/{id}")]
        [HttpPut]
        public void VoidComplexParamAndReplaceAction(string id, MyEmptyTestClass complex)
        {
        }
    }
}
