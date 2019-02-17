using AngularClientGenerator.Contracts.Descriptors;

namespace AngularClientGenerator.DescriptionParts
{
    public class ParameterDescription : TypeDescriptionPart
    {
        public string ParameterName { get; set; }
        public bool IsOptional { get; set; }

        public ParameterDescription(ParameterDescriptor parameterDescriptor): base(parameterDescriptor.ParameterType)
        {
            this.ParameterName = parameterDescriptor.ParameterName;
            this.IsOptional = parameterDescriptor.IsOptional;
        }
    }
}
