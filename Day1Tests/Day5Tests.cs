﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day5;

namespace UnitTests
{
    [TestClass]
    public class Day5Tests
    {
        [TestMethod]
        public void TestMinimumSeedLocation()
        {
            var seedToLocation = new SeedToLocation("Day5TestInput.txt");
            var result = seedToLocation.Process();

            Assert.AreEqual(35, result);
        }

        [TestMethod]
        public void TestMinimumSeedLocationFullData()
        {
            var seedToLocation = new SeedToLocation("Day5Input.txt");
            var result = seedToLocation.Process();

            Assert.AreEqual(825516882, result);
        }
        
    }
}
