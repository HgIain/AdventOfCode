using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18.Tests
{
    [TestClass()]
    public class Day18Tests
    {
        [TestMethod()]
        public void GetHoleSizeTest()
        {
            var poolDigger = new PoolDigger("Day18TestInput.txt", false);
            var result = poolDigger.GetHoleSize();
            Assert.AreEqual(62, result);
        }
        [TestMethod()]
        public void GetHoleSizeTestFullData()
        {
            var poolDigger = new PoolDigger("Day18Input.txt", false);
            var result = poolDigger.GetHoleSize();
            Assert.AreEqual(92758, result);
        }

        [TestMethod()]
        public void GetHoleSizeTestBig()
        {
            var poolDigger = new PoolDigger("Day18TestInput.txt", true);
            var result = poolDigger.GetHoleSize();
            Assert.AreEqual(952408144115, result);
        }
        [TestMethod()]
        public void GetHoleSizeTestFullDataBig()
        {
            var poolDigger = new PoolDigger("Day18Input.txt", true);
            var result = poolDigger.GetHoleSize();
            Assert.AreEqual(62762509300678, result);
        }
    }
}