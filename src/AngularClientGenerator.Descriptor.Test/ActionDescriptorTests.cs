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
    public class ActionDescriptorTests : TestBase
    {
        private ActionDescriptor GetActionDescriptor(string methodName)
        {
            return this.ApiDescriptor.ControllerDescriptors.First().ActionDescriptors.Single(ad => ad.Name == methodName);
        }

        [TestCase]
        public void SimpleActionDescriptorTest()
        {
            RegisterController<SimpleController>();

            RunInScope(() =>
            {
                Assert.AreEqual(1, this.ApiDescriptor.ControllerDescriptors.Count());

                var actionDescriptor = GetActionDescriptor(nameof(SimpleController.ActionOne));

                Assert.AreEqual("ActionOne", actionDescriptor.Name);
                Assert.AreEqual("api/simple/one", actionDescriptor.UrlTemplate);
                Assert.AreEqual("POST", actionDescriptor.HttpMethod.ToString());
                Assert.AreEqual(Enumerable.Empty<ParameterDescriptor>(), actionDescriptor.ParameterDescriptors);
                Assert.AreEqual(typeof(IHttpActionResult), actionDescriptor.ReturnValueDescriptor.Type);
            });
        }
    }
}
