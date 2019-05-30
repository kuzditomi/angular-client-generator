using System;
using System.Collections.Generic;
using AngularClientGenerator.Contracts;
using AngularClientGenerator.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AngularClientGenerator.Test.TsApiVisitorTests
{
    [TestClass]
    public abstract class GeneratedActionTestBase : TestBaseWithHelper
    {
        public GeneratedActionTestBase(ClientType clientType) : base(clientType)
        {
        }

        [TestMethod]
        public abstract void GeneratedVoidParameterlessActionTest();

        [TestMethod]
        public abstract void GeneratedVoidStringparamActionTest();

        [TestMethod]
        public abstract void GeneratedVoidComplexparamActionTest();

        [TestMethod]
        public abstract void StringReturnActionTest();

        [TestMethod]
        public abstract void ResponseTypeAttributeReturnActionTest();

        [DataTestMethod]
        [DataRow(typeof(IEnumerable<MyEmptyTestClass>))]
        [DataRow(typeof(List<MyEmptyTestClass>))]
        [DataRow(typeof(MyEmptyTestClass[]))]
        public abstract void ArrayReturnActionTest(Type arrayType);

        [TestMethod]
        public abstract void GenericTypeReturnActionTest();
    }
}