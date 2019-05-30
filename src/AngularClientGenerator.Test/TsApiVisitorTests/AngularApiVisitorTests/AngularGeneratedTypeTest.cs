using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator.Contracts;

namespace AngularClientGenerator.Test.TsApiVisitorTests.AngularApiVisitorTests
{
    [TestClass]
    public class AngularGeneratedTypeTest : GeneratedTypeTestBase
    {
        public AngularGeneratedTypeTest() : base(ClientType.Angular)
        {
        }

        protected override string FormatMethodHeader(string methodName, string parameterList, string returnType)
        {
            return $"public {methodName}({parameterList}): Observable<{returnType}> {{";
        }
    }
}
