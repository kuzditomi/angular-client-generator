using System.Collections.Generic;
using System.Net.Http;

namespace AngularClientGenerator.Contracts.Descriptors
{
    public class ActionDescriptor
    {
        public string Name { get; set; }
        public string UrlTemplate { get; set; }
        public HttpMethod HttpMethod { get; set; }

        public IEnumerable<ParameterDescriptor> ParameterDescriptors { get; set; }
        public TypeDescriptor ReturnValueDescriptor { get; set; }
    }
}
