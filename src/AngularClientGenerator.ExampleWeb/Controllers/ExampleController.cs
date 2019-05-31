using System.Web.Http;
using System.Web.Http.Description;
using AngularClientGenerator.ExampleWeb.Models;

namespace AngularClientGenerator.ExampleWeb.Controllers
{
    [RoutePrefix("api/example")]
    public class ExampleController: ApiController
    {
        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(ExampleModel))]
        public IHttpActionResult ExampleMethod(int id)
        {
            var model = new ExampleModel
            {
                Message= "Hello generator!"
            };

            return Ok(model);
        }
    }
}