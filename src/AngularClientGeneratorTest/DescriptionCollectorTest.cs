using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AngularClientGenerator;
using System.Web.Http.Description;
using System.Web.Http;
using System.IO;
using System.Linq;
using AngularClientGeneratorTest.TestControllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using AngularClientGeneratorTest.Util;
using Microsoft.Owin.Hosting;
using Owin;

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
                var descriptionCollector = new DescriptionCollector(this.ApiExplorer);
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
                var descriptionCollector = new DescriptionCollector(this.ApiExplorer);
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
                var descriptionCollector = new DescriptionCollector(this.ApiExplorer);
                var controllerDescription = descriptionCollector.GetControllerDescriptions().First(c => c.Name == "Simple");
                var actionDescriptions = descriptionCollector.GetActionDescriptionsForController(controllerDescription.Name);

                Assert.AreEqual(3, actionDescriptions.Count());
            });
        }
    }
}
