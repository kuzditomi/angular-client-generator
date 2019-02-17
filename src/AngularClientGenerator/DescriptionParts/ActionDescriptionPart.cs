using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AngularClientGenerator.Contracts.Descriptors;
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

        public ActionDescriptionPart(ActionDescriptor actionDescriptor)
        {
            this.Name = actionDescriptor.Name;
            this.UrlTemplate = actionDescriptor.UrlTemplate;
            this.HttpMethod = actionDescriptor.HttpMethod;

            this.ParameterDescriptions = actionDescriptor.ParameterDescriptors
                .Select(p => new ParameterDescription(p)).ToList();
            this.ReturnValueDescription = new TypeDescriptionPart(actionDescriptor.ReturnValueDescriptor.Type);
        }

        public void Accept(IApiVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
