using AngularClientGenerator.Contracts.Descriptors;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;

namespace AngularClientGenerator.Core.Descriptor
{
    public class ApiDescriptorConverter
    {
        public static ApiDescriptor CreateApiDescriptor(IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider)
        {
            var apiDescriptor = new ApiDescriptor
            {
                ControllerDescriptors = Enumerable.Empty<ControllerDescriptor>() // GetControllerDescriptors(apiExplorer)
            };

            return apiDescriptor;
        }
    }
}
