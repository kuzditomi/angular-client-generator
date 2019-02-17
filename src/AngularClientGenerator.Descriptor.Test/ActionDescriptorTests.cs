using System;
using AngularClientGenerator.Contracts.Descriptors;
using AngularClientGenerator.Descriptor.Test.TestControllers;
using NUnit.Framework;
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

        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        [TestCase("DELETE")]
        public void HttpMethodTest(string method)
        {
            RegisterController<HttpMethodController>();

            RunInScope(() =>
            {
                var actionDescriptor = GetActionDescriptor($"Test{method}");

                Assert.AreEqual(method, actionDescriptor.HttpMethod.ToString());
            });
        }

        [TestCase("Void", typeof(void))]
        [TestCase("Int", typeof(int))]
        [TestCase("Result", typeof(IHttpActionResult))]
        public void HttpMethodNoResponseType(string method, Type expectedReturnType)
        {
            RegisterController<ReturnValueController>();

            RunInScope(() =>
            {
                var actionDescriptor = GetActionDescriptor($"Test{method}");

                Assert.AreEqual(expectedReturnType, actionDescriptor.ReturnValueDescriptor.Type);
            });
        }

        [TestCase]
        public void HttpMethodResponseTypeOverReturnType()
        {
            RegisterController<ReturnValueController>();

            RunInScope(() =>
            {
                var actionDescriptor = GetActionDescriptor("TestResponseType");

                Assert.AreEqual(typeof(int), actionDescriptor.ReturnValueDescriptor.Type);
            });
        }

        [TestCase]
        public void ParameterTypes()
        {
            RegisterController<SimpleController>();

            RunInScope(() =>
            {
                var actionDescriptor = GetActionDescriptor("ActionTwo");

                var expectedTypes = new[] {typeof(int), typeof(string)};
                CollectionAssert.AreEquivalent(expectedTypes, actionDescriptor.ParameterDescriptors.Select(p => p.ParameterType));

                var expectedName = new[] { "a", "b" };
                CollectionAssert.AreEquivalent(expectedName, actionDescriptor.ParameterDescriptors.Select(p => p.ParameterName));

                var expectedOptional = new[] { false, false };
                CollectionAssert.AreEquivalent(expectedOptional, actionDescriptor.ParameterDescriptors.Select(p => p.IsOptional));
            });
        }


        [TestCase]
        public void OptionalParameterTypes()
        {
            RegisterController<SimpleController>();

            RunInScope(() =>
            {
                var actionDescriptor = GetActionDescriptor("ActionThree");

                Assert.That(actionDescriptor.ParameterDescriptors.First().IsOptional);
            });
        }
    }
}
