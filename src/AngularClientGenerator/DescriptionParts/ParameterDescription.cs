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
