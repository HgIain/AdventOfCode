using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day19;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19.Tests
{
    [TestClass()]
    public class FactoryProcessorTests
    {
        [TestMethod()]
        public void GetValueTest()
        {
            var factoryProcessor = new FactoryProcessor("Day19TestInput.txt", false);
            var result = factoryProcessor.GetValue();
            Assert.AreEqual((ulong)19114, result);
        }
        
        [TestMethod()]
        public void GetValueTestFullData()
        {
            var factoryProcessor = new FactoryProcessor("Day19Input.txt", false);
            var result = factoryProcessor.GetValue();
            Assert.AreEqual((ulong)398527, result);
        }

        [TestMethod()]
        public void GetValueTestRange()
        {
            var factoryProcessor = new FactoryProcessor("Day19TestInput.txt", true);
            var result = factoryProcessor.GetValue();
            Assert.AreEqual((ulong)167409079868000, result);
        }

        [TestMethod()]
        public void GetValueTestRangeFullData()
        {
            var factoryProcessor = new FactoryProcessor("Day19Input.txt", true);
            var result = factoryProcessor.GetValue();
            Assert.AreEqual((ulong)133973513090020, result);
        }
    }
}