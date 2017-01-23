﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using AngularClientGenerator.ExampleWebAPI.Models;

namespace AngularClientGenerator.ExampleWebAPI.Controllers
{
    [RoutePrefix("api/example")]
    public class ExampleController: ApiController
    {
        [Route("example")]
        [HttpGet]
        [ResponseType(typeof(ExampleModel))]
        public IHttpActionResult ExampleMethod()
        {
            var model = new ExampleModel
            {
                Message= "Hello generator!"
            };

            return Ok(model);
        }
    }
}