using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using AngularClientGenerator.Config;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Visitor;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class ClientBuilderTest
    {
        private ClientBuilder ClientBuilder { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var generatorConfig = new GeneratorConfig();
            this.ClientBuilder = new ClientBuilder(generatorConfig);
        }

        [TestMethod]
        public void Write_SimpleTest()
        {
            var expectedContent = "asd asd";
            ClientBuilder.Write(expectedContent);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void ClientBuilder_EmptyTest()
        {
            var content = ClientBuilder.GetContent();

            Assert.AreEqual(string.Empty, content);
        }

        [TestMethod]
        public void WriteLine_SimpleTest()
        {
            var line = "asda sd";
            var expectedContent = line + Environment.NewLine;
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void WriteLine_MultipleLineTest()
        {
            var line = "a sdas daa sd";
            var expectedContent = line + Environment.NewLine + line + Environment.NewLine + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.WriteLine(line);
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Write_WriteLine_MixedTest()
        {
            var line = "as dasdaa sd";
            var expectedContent = line + line + Environment.NewLine + line + Environment.NewLine + line + Environment.NewLine + line + line;

            ClientBuilder.Write(line);
            ClientBuilder.WriteLine(line);
            ClientBuilder.WriteLine(line);
            ClientBuilder.WriteLine(line);
            ClientBuilder.Write(line);
            ClientBuilder.Write(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Write_EmptyTest()
        {
            var line = "asd fsadf sad fsa ";
            var expectedContent = line + line;

            ClientBuilder.Write(line);
            ClientBuilder.Write();
            ClientBuilder.Write(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void WriteLine_EmptyTest()
        {
            var line = "a sdfasd fsad fsadf sadf";
            var expectedContent = line + Environment.NewLine + Environment.NewLine + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.WriteLine();
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Write_SingleParamTest()
        {
            var line = "aaa{0}aaa sa dasd asf";
            var param = "PARAM";
            var expectedContent = "aaaPARAMaaa sa dasd asf";

            ClientBuilder.Write(line, param);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Write_MixedParamTest()
        {
            var line = "aaa{0}aaa sa {1}dasd {2}asf{0}";
            var param1 = "PARAM";
            var param2 = 4;
            var param3 = true;
            var expectedContent = "aaaPARAMaaa sa 4dasd TrueasfPARAM";

            ClientBuilder.Write(line, param1, param2, param3);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void WriteLine_SingleParamTest()
        {
            var line = "aaa{0}aaa sa dasd asf";
            var param = "PARAM";
            var expectedContent = "aaaPARAMaaa sa dasd asf" + Environment.NewLine;

            ClientBuilder.WriteLine(line, param);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void WriteLine_MixedParamTest()
        {
            var line = "aaa{0}aaa sa {1}dasd {2}asf{0}";
            var param1 = "PARAM";
            var param2 = 4;
            var param3 = true;
            var expectedContent = "aaaPARAMaaa sa 4dasd TrueasfPARAM" + Environment.NewLine;

            ClientBuilder.WriteLine(line, param1, param2, param3);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_NoIndendTest()
        {
            var line = "aaa";
            var expectedContent = line;

            ClientBuilder.Write(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_IncreaseIndentTest()
        {
            var line = "aaa";
            var expectedContent = '\t' + line;

            ClientBuilder.IncreaseIndent();
            ClientBuilder.Write(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_DecreaseIndentTest()
        {
            var line = "aaa";
            var expectedContent = '\t' + line;

            ClientBuilder.IncreaseIndent();
            ClientBuilder.IncreaseIndent();
            ClientBuilder.DecreaseIndent();
            ClientBuilder.Write(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Indent_DecreaseIndentExceptionTest()
        {
            ClientBuilder.DecreaseIndent();
        }

        [TestMethod]
        public void Indent_MultipleIncreaseAndDecreaseTest()
        {
            var line = "aaa";
            var expectedContent =
                line + Environment.NewLine +
                "\t" + line + Environment.NewLine +
                line + Environment.NewLine +
                "\t" + line + Environment.NewLine +
                "\t\t" + line + Environment.NewLine +
                "\t\t\t" + line + Environment.NewLine +
                "\t" + line + Environment.NewLine +
                line + Environment.NewLine +
                "\t" + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.DecreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.DecreaseIndent(); ClientBuilder.DecreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.DecreaseIndent(); ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent(); ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_TypeDefaultTest()
        {
            var line = "aaa";
            var expectedContent = line + Environment.NewLine +
                "\t" + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent();
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_TypeTabTest()
        {
            ClientBuilder = new ClientBuilder();

            var line = "aaa";
            var expectedContent = line + Environment.NewLine +
                "\t" + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent();
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_TypeTwoSpaceTest()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.TwoSpace
            };

            ClientBuilder = new ClientBuilder(config);

            var line = "aaa";
            var expectedContent = line + Environment.NewLine +
                "  " + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent();
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }

        [TestMethod]
        public void Indent_TypeFourSpaceTest()
        {
            var config = new GeneratorConfig
            {
                IndentType = IndentType.FourSpace
            };

            ClientBuilder = new ClientBuilder(config);

            var line = "aaa";
            var expectedContent = line + Environment.NewLine +
                "    " + line + Environment.NewLine;

            ClientBuilder.WriteLine(line);
            ClientBuilder.IncreaseIndent();
            ClientBuilder.WriteLine(line);

            var content = ClientBuilder.GetContent();

            Assert.AreEqual(expectedContent, content);
        }
    }
}
