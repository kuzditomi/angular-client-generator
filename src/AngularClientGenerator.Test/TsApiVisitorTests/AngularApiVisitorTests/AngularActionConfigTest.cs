using AngularClientGenerator.Contracts;
using AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularApiVisitorTests
{
    [TestClass]
    public class AngularActionConfigTest : ActionConfigTestBase
    {
        public AngularActionConfigTest() : base(ClientType.Angular)
        {
        }

        protected override string FormatConfigMethodHeader(string methodName, string parameterList)
        {
            return $"public {methodName}({parameterList}): RequestOptions {{";
        }

        protected override string FormatConfigMethodFooter()
        {
            return "} as RequestOptions;";
        }
    }
}
