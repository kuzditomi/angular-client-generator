using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public abstract class ApiVisitor : IApiVisitor
    {
        protected ClientBuilder ClientBuilder;
        protected IClientBuilderConfig Config;

        protected ApiVisitor(IClientBuilderConfig config)
        {
            this.Config = config;
            this.ClientBuilder = new ClientBuilder(config);
        }

        public string GetContent()
        {
            return ClientBuilder.GetContent();
        }

        public abstract void Visit(ControllerDescriptionPart controllerDescription);
        public abstract void Visit(ActionDescriptionPart actionDescription);
        public abstract void Visit(ModuleDescriptionPart moduleDescription);
    }
}
