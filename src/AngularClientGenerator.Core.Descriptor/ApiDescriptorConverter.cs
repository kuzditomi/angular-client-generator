using AngularClientGenerator.Contracts.Descriptors;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AngularClientGenerator.Core.Descriptor
{
    public class ApiDescriptorConverter
    {
        public static ApiDescriptor CreateApiDescriptor(IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider)
        {
            var descriptorGroups = apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items;
            var controllers = new Dictionary<string, List<ApiDescription>>();

            foreach (var item in descriptorGroups.SelectMany(group => group.Items))
            {
                if (!(item.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
                {
                    throw new ArgumentNullException(nameof(item.ActionDescriptor), "Unexpected descriptor type");
                }

                if (!controllers.ContainsKey(controllerActionDescriptor.ControllerName))
                {
                    controllers[controllerActionDescriptor.ControllerName] = new List<ApiDescription>();
                }

                controllers[controllerActionDescriptor.ControllerName].Add(item);
            }

            var apiDescriptor = new ApiDescriptor
            {
                ControllerDescriptors = controllers.Select(c => new ControllerDescriptor
                {
                    Name = c.Key,
                    ActionDescriptors = c.Value.Select(a => new ActionDescriptor
                    {
                        Name = (a.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor) ? controllerActionDescriptor.ActionName : string.Empty,
                        HttpMethod = new System.Net.Http.HttpMethod(a.HttpMethod),
                        UrlTemplate = a.RelativePath,
                        ReturnValueDescriptor = new TypeDescriptor { Type = a.SupportedResponseTypes.First().Type },
                        ParameterDescriptors = a.ParameterDescriptions.Select(p => new ParameterDescriptor
                        {
                            ParameterName = p.Name,
                            ParameterType = p.Type,
                            IsOptional = !p.ModelMetadata.IsRequired
                        })
                    })
                })
            };

            return apiDescriptor;
        }
    }
}
