using System;

namespace AngularClientGenerator.Contracts.Descriptors
{
    public class ParameterDescriptor
    {
        public Type ParameterType { get; }
        public string ParameterName { get; }
        public bool IsOptional { get; }
    }
}
