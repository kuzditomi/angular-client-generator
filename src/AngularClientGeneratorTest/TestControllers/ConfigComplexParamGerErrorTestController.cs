using System.Web.Http;
using AngularClientGeneratorTest.TestModels;
using SB.TradingTools.AngularClientGeneratorTest.TestModels;

namespace AngularClientGeneratorTest.TestControllers
{
    [RoutePrefix("api/configerrortest")]
    public class ConfigComplexParamGerErrorTestController: ApiController
    {
        [Route("voidcomplexparamandreplacegeterror/{id}")]
        [HttpGet]
        public void VoidComplexParamAndReplaceGetAction(string id, [FromUri]MyEmptyTestClass complex, int a)
        {
        }
    }
}
