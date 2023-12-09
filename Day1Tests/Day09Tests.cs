using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day09;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09.Tests
{
    [TestClass()]
    public class Day09Tests
    {
        [TestMethod()]
        public void ProcessorTest()
        {
            var total = Differentiator.Processor("Day9TestInput.txt");
            Assert.AreEqual(114,total);
        }
        [TestMethod()]
        public void ProcessorTestFullData()
        {
            var total = Differentiator.Processor("Day9Input.txt");
            Assert.AreEqual(1681758908, total);
        }

        [TestMethod()]
        public void ProcessorTestBackwards()
        {
            var total = Differentiator.Processor("Day9TestInput.txt", true);
            Assert.AreEqual(2, total);
        }
        [TestMethod()]
        public void ProcessorTestBackwardsFullData()
        {
            var total = Differentiator.Processor("Day9Input.txt", true);
            Assert.AreEqual(803, total);
        }
    }
}