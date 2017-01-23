using System;
using System.IO;

namespace AngularClientGenerator.Visitor
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
            var directory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(directory, "StaticParts", filename);

            if(!File.Exists(filePath))
                throw new ArgumentException("Invalid file path to build static part from.", nameof(filePath));

            var content = File.ReadAllLines(filePath);

            foreach (var line in content)
            {
                builder.WriteLine(line.Replace("{", "{{").Replace("}","}}"));
            }
        }
    }
}
