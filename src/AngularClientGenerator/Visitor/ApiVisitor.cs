using AngularClientGenerator.Contracts.Config;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public abstract class ApiVisitor : IApiVisitor
    {
        protected ClientBuilder ClientBuilder;
        private readonly StaticPartBuilder StaticPartBuilder;
        protected IVisitorConfig Config;

        protected ApiVisitor(IVisitorConfig config, ClientBuilder builder)
        {
            this.Config = config;
            this.ClientBuilder = builder;
            this.StaticPartBuilder = new StaticPartBuilder(builder);
        }

        public string GetContent()
        {
            return ClientBuilder.GetContent();
        }

        public abstract void Visit(ControllerDescriptionPart controllerDescription);
        public abstract void Visit(ActionDescriptionPart actionDescription);
        public abstract void Visit(ModuleDescriptionPart moduleDescription);
        public abstract void Visit(TypeDescriptionPart typeDescriptionPart);

        /// <summary>
        /// Writes the content of the file to contentbuilder
        /// </summary>
        /// <param name="filename">Name of the file under StaticParts folder. example: "EnumHelper.ts.template"</param>
        protected void WriteStaticPart(string filename)
        {
            this.StaticPartBuilder.WritePart(filename);
        }
    }
}
