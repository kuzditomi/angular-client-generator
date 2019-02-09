using AngularClientGenerator.Contracts.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace AngularClientGenerator.Descriptor
{
    public class ApiDescriptorConverter
    {
        public static ApiDescriptor CreateApiDescriptor(IApiExplorer apiExplorer)
        {
            var apiDescriptor = new ApiDescriptor
            {
                ControllerDescriptors = GetControllerDescriptors(apiExplorer)
            };

            return apiDescriptor;
        }

        private static IEnumerable<ControllerDescriptor> GetControllerDescriptors(IApiExplorer apiExplorer)
        {
            return apiExplorer.ApiDescriptions
                .Select(d => d.ActionDescriptor.ControllerDescriptor)
                .Distinct()
                .Select(cd => CreateControllerDescriptor(apiExplorer, cd))
                .ToList();

        }

        private static IEnumerable<ActionDescriptor> GetActionDescriptorForController(IApiExplorer apiExplorer, string controllerName)
        {
            return apiExplorer.ApiDescriptions
                .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == controllerName)
                .Select(ad => CreateActionDescriptor(apiExplorer, ad));
        }

        private static ControllerDescriptor CreateControllerDescriptor(IApiExplorer apiExplorer, HttpControllerDescriptor description)
        {
            return new ControllerDescriptor
            {
                Name = description.ControllerName,
                ActionDescriptors = GetActionDescriptorForController(apiExplorer, description.ControllerName)
            };
        }

        private static ActionDescriptor CreateActionDescriptor(IApiExplorer apiExplorer, ApiDescription description)
        {
            var actionDescriptor = new ActionDescriptor();

            var reflectedDescriptor = description.ActionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedDescriptor == null)
                throw new ArgumentNullException(nameof(description), "Unexpected descriptor type");

            actionDescriptor.Name = description.ActionDescriptor.ActionName;
            actionDescriptor.UrlTemplate = description.Route.RouteTemplate;

            var responseTypeAttribute = reflectedDescriptor.MethodInfo.GetCustomAttribute(typeof(ResponseTypeAttribute));
            if (responseTypeAttribute == null)
            {
                actionDescriptor.ReturnValueDescriptor = new TypeDescriptor { Type = (reflectedDescriptor.MethodInfo.ReturnType) };
            }
            else
            {
                actionDescriptor.ReturnValueDescriptor = new TypeDescriptor { Type = (((ResponseTypeAttribute)responseTypeAttribute).ResponseType) };
            }

            actionDescriptor.HttpMethod = description.HttpMethod;

            actionDescriptor.ParameterDescriptors = description.ParameterDescriptions
                .Select(p => new ParameterDescriptor
                {
                    ParameterType = p.ParameterDescriptor.ParameterType,
                    ParameterName = p.Name,
                    IsOptional = p.ParameterDescriptor.IsOptional
                }).ToList();

            return actionDescriptor;
        }
    }
}
