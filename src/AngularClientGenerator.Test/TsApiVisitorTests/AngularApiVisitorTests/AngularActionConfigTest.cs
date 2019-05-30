using AngularClientGenerator.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class AngularActionConfigTest : ActionConfigTestBase
    {
        public AngularActionConfigTest() : base(ClientType.Angular)
        {
        }

        protected override string FormatConfigMethodHeader(string methodName, string parameterList)
        {
            return $"public {methodName}({parameterList}): HttpRequest {{";
        }
    }
}
