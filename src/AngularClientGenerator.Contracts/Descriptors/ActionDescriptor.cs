using System.Collections.Generic;
using System.Net.Http;

namespace AngularClientGenerator.Contracts.Descriptors
{
    public class ActionDescriptor
    {
        public string Name { get; }
        public string UrlTemplate { get; }
        public HttpMethod HttpMethod { get; }

        public IEnumerable<ParameterDescriptor> ParameterDescriptors { get; }
        public TypeDescriptor ReturnValueDescriptor { get; }
    }
}
