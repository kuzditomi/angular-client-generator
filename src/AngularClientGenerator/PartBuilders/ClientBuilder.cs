using System;
using System.Text;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Contracts.Config;

namespace AngularClientGenerator.PartBuilders
{
    public class ClientBuilder
    {
        private int IndentCounter;
        private StringBuilder Builder;
        private IClientBuilderConfig Config { get; }

        private string Indent
        {
            get
            {
                if (IndentCounter == 0)
                    return string.Empty;

                switch (Config.IndentType)
                {
                    case IndentType.TwoSpace:
                        return new string(' ', IndentCounter * 2);
                    case IndentType.FourSpace:
                        return new string(' ', IndentCounter * 4);
                    case IndentType.Tab:
                        return new string('\t', IndentCounter);
                    default:
                        throw new InvalidOperationException("No indent type specified");
                }
            }
        }

        public ClientBuilder() : this(new GeneratorConfig())
        {
        }

        public ClientBuilder(IClientBuilderConfig config)
        {
            IndentCounter = 0;
            Builder = new StringBuilder();
            Config = config;
        }

        public void IncreaseIndent()
        {
            IndentCounter++;
        }

        public void DecreaseIndent()
        {
            if (IndentCounter == 0)
                throw new InvalidOperationException("Indent is already on 0");

            IndentCounter--;
        }

        public void Write(string pattern = "", params object[] parameters)
        {
            Builder.Append(Indent);
            Builder.AppendFormat(pattern, parameters);
        }

        public void WriteLine(string pattern = "", params object[] parameters)
        {
            if (!string.IsNullOrEmpty(pattern))
                Builder.Append(Indent);

            Builder.AppendFormat(pattern, parameters);
            Builder.AppendLine();
        }

        public string GetContent()
        {
            return Builder.ToString();
        }
    }
}
