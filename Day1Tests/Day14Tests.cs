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
            var result = tilter.GetTotalStress();
            Assert.AreEqual(136, result);
        }

        [TestMethod()]
        public void TilterTestFullData()
        {
            var tilter = new Tilter("Day14Input.txt");
            var result = tilter.GetTotalStress();
            Assert.AreEqual(108759, result);
        }
    }
}