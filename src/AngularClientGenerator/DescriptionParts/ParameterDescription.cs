using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace AngularClientGenerator.DescriptionParts
{
    public class ParameterDescription : TypeDescriptionPart
    {
        public string ParameterName { get; set; }

        public ParameterDescription(HttpParameterDescriptor parameterDescriptor): base(parameterDescriptor.ParameterType)
        {
            this.ParameterName = parameterDescriptor.ParameterName;
        }
    }
}
