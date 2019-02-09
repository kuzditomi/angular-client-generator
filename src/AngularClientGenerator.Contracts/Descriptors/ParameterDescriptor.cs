using System;

namespace AngularClientGenerator.Contracts.Descriptors
{
    public class ParameterDescriptor
    {
        public Type ParameterType { get; set; }
        public string ParameterName { get; set; }
        public bool IsOptional { get; set; }
    }
}
