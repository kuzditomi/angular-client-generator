using System;

namespace AngularClientGenerator.Contracts.Config
{
    public interface IVisitorConfig
    {
        string ModuleName { get; set; }
        bool UseNamespaces { get; set; }
        string DefaultBaseUrl { get; set; }
        string UrlSuffix { get; set; }

        /// <summary>
        /// Should return the desired namespace of a type, eg. from type Examle.Namespace.Models.Car it can return Models or MyNameSpace.Models, etc"
        /// </summary>
        Func<Type, string> NamespaceNamingRule { get; set; }
    }
}
