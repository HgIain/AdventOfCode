using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day21;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21.Tests
{
    [TestClass()]
    public class Day21Tests
    {
        [TestMethod()]
        public void GetPossibleEndingsTest()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(6, false);
            Assert.AreEqual(16, result);
        }

        [TestMethod()]
        public void GetPossibleEndingsTestFullData()
        {
            var pathWalker = new PathWalker("Day21Input.txt");
            var result = pathWalker.GetPossibleEndings(64, false);
            Assert.AreEqual(3605, result);
        }

        [TestMethod()]
        public void GetPossibleEndingsTestInfinite()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(6, true);
            Assert.AreEqual(16, result);
        }
        [TestMethod()]
        public void GetPossibleEndingsTestInfinite2()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(10, true);
            Assert.AreEqual(50, result);
        }
        [TestMethod()]
        public void GetPossibleEndingsTestInfinite3()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(50, true);
            Assert.AreEqual(1594, result);
        }
        [TestMethod()]
        public void GetPossibleEndingsTestInfinite4()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(100, true);
            Assert.AreEqual(6536, result);
        }
        [TestMethod()]
        public void GetPossibleEndingsTestInfinite5()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(500, true);
            Assert.AreEqual(167004, result);
        }
        [TestMethod()]
        public void GetPossibleEndingsTestInfinite6()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(1000, true);
            Assert.AreEqual(668697, result);
        }
        [TestMethod()]
        public void GetPossibleEndingsTestInfinite7()
        {
            var pathWalker = new PathWalker("Day21TestInput.txt");
            var result = pathWalker.GetPossibleEndings(5000, true);
            Assert.AreEqual(16733044, result);
        }

    }
}