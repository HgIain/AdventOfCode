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
            var factoryProcessor = new FactoryProcessor("Day19TestInput.txt");
            var result = factoryProcessor.GetValue();
            Assert.AreEqual(19114, result);
        }
        
        [TestMethod()]
        public void GetValueTestFullData()
        {
            var factoryProcessor = new FactoryProcessor("Day19Input.txt");
            var result = factoryProcessor.GetValue();
            Assert.AreEqual(398527, result);
        }
    }
}