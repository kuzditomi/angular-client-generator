using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace AngularClientGenerator.DescriptionParts
{
    public class ParameterDescription : TypeDescriptionPart
    {
        public string ParameterName { get; set; }
        public bool IsOptional { get; set; }

        public ParameterDescription(ApiParameterDescription parameterDescriptor): base(parameterDescriptor.ParameterDescriptor.ParameterType)
        {
            this.ParameterName = parameterDescriptor.Name;
            this.IsOptional = parameterDescriptor.ParameterDescriptor.IsOptional;
        }
    }
}
