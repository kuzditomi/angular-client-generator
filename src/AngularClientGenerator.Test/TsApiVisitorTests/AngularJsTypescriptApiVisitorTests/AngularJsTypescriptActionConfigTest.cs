using AngularClientGenerator.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class AngularJsTypescriptActionConfigTest : ActionConfigTestBase
    {
        public AngularJsTypescriptActionConfigTest() : base(ClientType.AngularJsTypeScript)
        {
        }

        protected override string FormatConfigMethodHeader(string methodName, string parameterList)
        {
            return $"public {methodName}({parameterList}): ng.IRequestConfig {{";
        }

        protected override string FormatConfigMethodFooter()
        {
            return "};";
        }
    }
}
