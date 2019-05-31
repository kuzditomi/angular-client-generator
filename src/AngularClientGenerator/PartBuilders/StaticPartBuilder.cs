using AngularClientGenerator.Visitors;
using System.IO;
using System.Reflection;

namespace AngularClientGenerator.PartBuilders.Visitor
{
    public class StaticPartBuilder
    {
        private ClientBuilder builder;

        public StaticPartBuilder(ClientBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Writes static part from provided file with clientbuilder
        /// </summary>
        /// <param name="filename">Name of the file under StaticParts folder</param>
        public void WritePart(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AngularClientGenerator.StaticParts.EnumHelper.ts.template.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    builder.WriteLine(line.Replace("{", "{{").Replace("}", "}}"));
                }                
            }
        }
    }
}
