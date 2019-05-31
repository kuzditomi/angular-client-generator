using AngularClientGenerator.Contracts;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    public abstract class TestBaseWithHelper
    {
        protected ClientType clientType;
        protected TsApiVisitorTestHelper visitor;

        protected TestBaseWithHelper(ClientType clientType)
        {
            this.clientType = clientType;
            this.visitor = new TsApiVisitorTestHelper(clientType);
        }
    }
}
