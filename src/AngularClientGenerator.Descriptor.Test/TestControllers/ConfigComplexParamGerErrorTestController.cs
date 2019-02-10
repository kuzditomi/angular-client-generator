using System.Web.Http;
using AngularClientGenerator.Descriptor.Test.TestModels;

namespace AngularClientGenerator.Descriptor.Test.TestControllers
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
