using AngularClientGenerator.Descriptor.Test.TestControllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AngularClientGenerator.Descriptor.Test
{
    [TestFixture]
    public class ControllerDescriptorTests : TestBase
    {
        [TestCase]
        public void GenerateAllRegisteredControllers()
        {
            RegisterController<TestController>();
            RegisterController<SimpleController>();

            this.RunInScope(() =>
            {
                var actualControllerNames = this.ApiDescriptor.ControllerDescriptors.Select(c => c.Name).ToList();

                var expectedControllersNames = new List<string>
                {
                    "Test",
                    "Simple"
                };

                CollectionAssert.AreEquivalent(expectedControllersNames, actualControllerNames, "Description doesnt include all registered controller.");
            });
        }

        
    }
}
