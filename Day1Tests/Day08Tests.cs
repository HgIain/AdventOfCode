using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day08;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08.Tests
{
    [TestClass()]
    public class Day08Tests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var camelNetwork = new CamelNetwork("Day8TestInput.txt");
            var result = camelNetwork.Process();
            Assert.AreEqual(2, result);
        }

        [TestMethod()]
        public void ProcessTest2()
        {
            var camelNetwork = new CamelNetwork("Day8TestInput2.txt");
            var result = camelNetwork.Process();
            Assert.AreEqual(6, result);
        }

        [TestMethod()]
        public void ProcessTestFullData()
        {
            var camelNetwork = new CamelNetwork("Day8Input.txt");
            var result = camelNetwork.Process();
            Assert.AreEqual(18023, result);
        }

        [TestMethod()]
        public void ProcessMultiple()
        {
            var camelNetwork = new CamelNetwork("Day8TestInput3.txt");
            var result = camelNetwork.ProcessMultiple();
            Assert.AreEqual((ulong)6, result);
        }

        [TestMethod()]
        public void ProcessMultipleFullData()
        {
            var camelNetwork = new CamelNetwork("Day8Input.txt");
            var result = camelNetwork.ProcessMultiple ();
            Assert.AreEqual((ulong)14449445933179, result);
        }
    }
}