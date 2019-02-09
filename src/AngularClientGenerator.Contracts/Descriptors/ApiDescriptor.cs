using System.Collections.Generic;

namespace AngularClientGenerator.Contracts.Descriptors
{
    public class ApiDescriptor
    {
        public IEnumerable<ControllerDescriptor> ControllerDescriptors { get; }
    }
}
