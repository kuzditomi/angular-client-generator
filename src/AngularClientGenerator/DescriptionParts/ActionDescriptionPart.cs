using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.DescriptionParts
{
    public class ActionDescriptionPart: IDescriptionPart
    {
        public string Name { get; set; }
        public string UrlTemplate { get; set; }
        public HttpMethod HttpMethod { get; set; }

        public IEnumerable<ParameterDescription> ParameterDescriptions { get; set; }
        public TypeDescriptionPart ReturnValueDescription { get; set; }

        public ActionDescriptionPart(ApiDescription apiDescription)
        {
            var reflectedDescriptor = apiDescription.ActionDescriptor as ReflectedHttpActionDescriptor;
            if(reflectedDescriptor == null)
                throw new ArgumentException("Unexpected descriptor type", nameof(apiDescription));

            this.Name = apiDescription.ActionDescriptor.ActionName;
            this.UrlTemplate = apiDescription.Route.RouteTemplate;

            this.ReturnValueDescription = new TypeDescriptionPart(reflectedDescriptor.MethodInfo.ReturnType);
            this.HttpMethod = apiDescription.HttpMethod;

            this.ParameterDescriptions = apiDescription.ParameterDescriptions
                .Select(p => new ParameterDescription(p)).ToList();
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
