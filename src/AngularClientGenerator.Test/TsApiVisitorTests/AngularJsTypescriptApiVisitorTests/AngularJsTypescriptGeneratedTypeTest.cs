using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Contracts;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularJsTypescriptApiVisitorTests
{
    [TestClass]
    public class AngularJsTypescriptGeneratedTypeTest : GeneratedTypeTestBase
    {
        public AngularJsTypescriptGeneratedTypeTest() : base(ClientType.AngularJsTypeScript)
        {
        }

        protected override string FormatMethodHeader(string methodName, string parameterList, string returnType)
        {
            return $"public {methodName} = ({parameterList}): ng.IPromise<{returnType}> => {{";
        }
    }
}
