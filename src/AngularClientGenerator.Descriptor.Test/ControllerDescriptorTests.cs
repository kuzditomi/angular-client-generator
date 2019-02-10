using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Descriptor.Test.TestControllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

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
            RegisterController<ConfigVoidTestController>();
            RegisterController<GeneratedMethodTestController>();
            RegisterController<TypeTestController>();

            this.RunInScope(() =>
            {
                var actualControllerNames = this.ApiDescriptor.ControllerDescriptors.Select(c => c.Name).ToList();

                var expectedControllersNames = new List<string>
                {
                    "Test",
                    "Simple",
                    "ConfigVoidTest",
                    "GeneratedMethodTest",
                    "TypeTest"
                };

                CollectionAssert.AreEquivalent(expectedControllersNames, actualControllerNames, "Description doesnt include all registered controller.");
            });
        }
    }
}
