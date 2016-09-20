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
                .Select(cd =>
                {
                    var actionDescriptions = this.ApiExplorer.ApiDescriptions
                            .Where(a => a.ActionDescriptor.ControllerDescriptor.ControllerName == cd.ControllerName)
                            .Select(a => a.ActionDescriptor);

                    return new ControllerDescriptionPart(cd, actionDescriptions);
                }).ToList();
        }
    }
}
