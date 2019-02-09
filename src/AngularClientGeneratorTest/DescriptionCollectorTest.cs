using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using AngularClientGeneratorTest.TestControllers;

namespace AngularClientGeneratorTest
{
    [TestClass]
    public class DescriptionCollectorTest : TestBase
    {
        [TestMethod]
        public void GetControllerDescriptions_All()
        {
            this.RegisterController<TestController>();
            this.RegisterController<SimpleController>();

            this.RunInScope(() =>
            {
                var descriptionCollector = new DescriptionCollector(null);//this.ApiExplorer);
                var controllerDefinitions = descriptionCollector.GetControllerDescriptions();

                Assert.AreEqual(2, controllerDefinitions.Count());

                var collectedControllerTypes = controllerDefinitions.Select(c => c.Name).ToList();

                CollectionAssert.AreEquivalent(new List<string> { "Test", "Simple" }, collectedControllerTypes);
            });
        }

        [TestMethod]
        public void GetControllerDescriptions_Empty()
        {
            this.RunInScope(() =>
            {
                var descriptionCollector = new DescriptionCollector(null);// this.ApiExplorer);
                var controllerDefinitions = descriptionCollector.GetControllerDescriptions();

                Assert.AreEqual(0, controllerDefinitions.Count());
            });
        }

        [TestMethod]
        public void GetActionDescriptorsByController_All()
        {
            RegisterController<TestController>();
            RegisterController<SimpleController>();

            this.RunInScope(() =>
            {
                var descriptionCollector = new DescriptionCollector(null);//this.ApiExplorer);
                var controllerDescription = descriptionCollector.GetControllerDescriptions().First(c => c.Name == "Simple");
                var actionDescriptions = descriptionCollector.GetActionDescriptionsForController(controllerDescription.Name);

                Assert.AreEqual(3, actionDescriptions.Count());
            });
        }
    }
}
