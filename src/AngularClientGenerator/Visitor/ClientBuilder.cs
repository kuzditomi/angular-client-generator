using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Config;

namespace AngularClientGenerator.Visitor
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

                switch (this.Config.IndentType)
                {
                    case IndentType.TwoSpace:
                        return new String(' ', IndentCounter * 2);
                    case IndentType.FourSpace:
                        return new String(' ', IndentCounter * 4);
                    case IndentType.Tab:
                        return new String('\t', IndentCounter);
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
            this.IndentCounter = 0;
            this.Builder = new StringBuilder();
            this.Config = config;
        }

        public void IncreaseIndent()
        {
            this.IndentCounter++;
        }

        public void DecreaseIndent()
        {
            if (this.IndentCounter == 0)
                throw new InvalidOperationException("Indent is already on 0");

            this.IndentCounter--;
        }

        public void Write(string line = "")
        {
            this.Builder.Append(Indent);
            this.Builder.Append(line);
        }

        public void WriteLine(string line = "")
        {
            this.Builder.Append(Indent);
            this.Builder.AppendLine(line);
        }

        public void Write(string pattern, params object[] parameters)
        {
            this.Builder.Append(Indent);
            this.Builder.Append(String.Format(pattern, parameters));
        }

        public void WriteLine(string pattern, params object[] parameters)
        {
            this.Builder.Append(Indent);
            this.Builder.AppendLine(String.Format(pattern, parameters));
        }

        public string GetContent()
        {
            return Builder.ToString();
        }
    }
}
