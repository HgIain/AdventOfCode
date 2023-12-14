using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day14;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14.Tests
{
    [TestClass()]
    public class Day14Tests
    {
        [TestMethod()]
        public void TilterTest()
        {
            var tilter = new Tilter("Day14TestInput.txt");
            var result = tilter.GetTotalStress(false);
            Assert.AreEqual(136, result);
        }

        [TestMethod()]
        public void TilterTestFullData()
        {
            var tilter = new Tilter("Day14Input.txt");
            var result = tilter.GetTotalStress(false);
            Assert.AreEqual(108759, result);
        }

        [TestMethod()]
        public void TilterBigTest()
        {
            var tilter = new Tilter("Day14TestInput.txt");
            var result = tilter.GetTotalStress(true);
            Assert.AreEqual(64, result);
        }

        [TestMethod()]
        public void TilterBigTestFullData()
        {
            var tilter = new Tilter("Day14Input.txt");
            var result = tilter.GetTotalStress(true);
            Assert.AreEqual(89089, result);
        }
    }
}