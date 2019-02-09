using System;

namespace AngularClientGenerator.Contracts.Exceptions
{
    public class GeneratorConfigurationException : Exception
    {
        public GeneratorConfigurationException() : base()
        {

        }

        public GeneratorConfigurationException(string message) : base(message)
        {

        }
    }
}
