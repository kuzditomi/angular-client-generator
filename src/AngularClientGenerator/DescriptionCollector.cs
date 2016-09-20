using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator
{
    public class DescriptionCollector
    {
        private IApiExplorer ApiExplorer;

        public DescriptionCollector(IApiExplorer apiExplorer)
        {
            this.ApiExplorer = apiExplorer;
        }

        public IEnumerable<ControllerDescriptionPart> GetControllerDescriptions()
        {
            return this.ApiExplorer.ApiDescriptions
                .Select(d => d.ActionDescriptor.ControllerDescriptor)
                .Distinct()
                .Select(d => new ControllerDescriptionPart(d));
        }

        public IEnumerable<ActionDescriptionPart> GetActionDescriptorsByController(string controllerName)
        {
            return this.ApiExplorer.ApiDescriptions
                .Select(d => d.ActionDescriptor)
                .Where(a => a.ControllerDescriptor.ControllerName == controllerName)
                .Select(d => new ActionDescriptionPart(d))
                .ToList();
        }
    }
}
