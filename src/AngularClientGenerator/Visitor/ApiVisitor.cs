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
        protected IVisitorConfig Config;

        protected ApiVisitor(IVisitorConfig config, ClientBuilder builder)
        {
            this.Config = config;
            this.ClientBuilder = builder;
        }

        public string GetContent()
        {
            return ClientBuilder.GetContent();
        }

        public abstract void Visit(ControllerDescriptionPart controllerDescription);
        public abstract void Visit(ActionDescriptionPart actionDescription);
        public abstract void Visit(ModuleDescriptionPart moduleDescription);
        public abstract void Visit(TypeDescriptionPart typeDescriptionPart);
    }
}
