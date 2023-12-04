using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3.Tests
{
    [TestClass()]
    public class Day3Tests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var result = PartFinder.Process("day3testinput.txt");
            Assert.AreEqual(4361, result);
        }

        [TestMethod()]
        public void ProcessTestFullData()
        {
            var result = PartFinder.Process("day3input.txt");
            Assert.AreEqual(536576, result);
        }

        [TestMethod()]
        public void ProcessGearRatios()
        {
            var result = GearRatios.Process("day3testinput.txt");
            Assert.AreEqual(467835, result);
        }

        [TestMethod()]
        public void ProcessGearRatiosFullData()
        {
            var result = GearRatios.Process("day3input.txt");
            Assert.AreEqual(75741499, result);
        }        
    }
}