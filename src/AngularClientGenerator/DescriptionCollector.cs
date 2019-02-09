using System.Collections.Generic;
using System.Linq;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator
{
    public class DescriptionCollector
    {
        private ApiDescriptor ApiDescriptor;

        public DescriptionCollector(ApiDescriptor apiDescriptor)
        {
            this.ApiDescriptor = apiDescriptor;
        }

        public IEnumerable<ControllerDescriptionPart> GetControllerDescriptions()
        {
            return this.ApiDescriptor.ControllerDescriptors
                .Select(cd => new ControllerDescriptionPart(cd)).ToList();
        }

        public IEnumerable<ActionDescriptionPart> GetActionDescriptionsForController(string controllerName)
        {
            return this.ApiDescriptor.ControllerDescriptors
                .Single(a => a.Name == controllerName)
                .ActionDescriptors
                .Select(a => new ActionDescriptionPart(a));
        }
    }
}
