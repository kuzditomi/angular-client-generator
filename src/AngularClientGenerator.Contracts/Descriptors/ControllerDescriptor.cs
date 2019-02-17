using System.Collections.Generic;

namespace AngularClientGenerator.Contracts.Descriptors
{
    public class ControllerDescriptor
    {
        public string Name { get; set; }
        public IEnumerable<ActionDescriptor> ActionDescriptors { get; set; }
    }
}
