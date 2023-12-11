using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11.Tests
{
    [TestClass()]
    public class Day11Tests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var result = Galaxy.Process("Day11TestInput.txt", 2);
            Assert.AreEqual(374, result);
        }
        [TestMethod()]
        public void ProcessTestFullData()
        {
            var result = Galaxy.Process("Day11Input.txt", 2);
            Assert.AreEqual(9742154, result);
        }
        [TestMethod()]
        public void ProcessTestBigExpansion()
        {
            var result = Galaxy.Process("Day11TestInput.txt", 10);
            Assert.AreEqual(1030, result);
        }
        [TestMethod()]
        public void ProcessTestBigExpansion10()
        {
            var result = Galaxy.Process("Day11TestInput.txt", 100);
            Assert.AreEqual(8410, result);
        }
        [TestMethod()]
        public void ProcessTestBigExpansionFullData()
        {
            var result = Galaxy.Process("Day11Input.txt", 1000000);
            Assert.AreEqual(411142919886, result);
        }
    }
}