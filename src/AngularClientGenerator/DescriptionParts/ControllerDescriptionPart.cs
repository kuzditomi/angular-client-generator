using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace AngularClientGenerator.DescriptionParts
{
    public class ControllerDescriptionPart
    {
        public ControllerDescriptionPart(HttpControllerDescriptor controllerDescriptor)
        {
            this.Name = controllerDescriptor.ControllerName;
        }

        public string Name { get; set; }
    }
}
