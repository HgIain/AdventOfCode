using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day16;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16.Tests
{
    [TestClass()]
    public class Day16Tests
    {
        [TestMethod()]
        public void GetEnergisedTileCountTest()
        {
            var pathFollower = new MirrorPathFollower("Day16TestInput.txt");
            var result = pathFollower.GetEnergisedTileCount(false);
            Assert.AreEqual(46, result);
        }

        [TestMethod()]
        public void GetEnergisedTileCountTestFullData()
        {
            var pathFollower = new MirrorPathFollower("Day16Input.txt");
            var result = pathFollower.GetEnergisedTileCount(false);
            Assert.AreEqual(7111, result);
        }

        [TestMethod()]
        public void GetMaxEnergisedTileCountTest()
        {
            var pathFollower = new MirrorPathFollower("Day16TestInput.txt");
            var result = pathFollower.GetEnergisedTileCount(true);
            Assert.AreEqual(51, result);
        }

        [TestMethod()]
        public void GetMaxEnergisedTileCountTestFullData()
        {
            var pathFollower = new MirrorPathFollower("Day16Input.txt");
            var result = pathFollower.GetEnergisedTileCount(true);
            Assert.AreEqual(7831, result);
        }
    }
}