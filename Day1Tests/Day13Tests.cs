using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day13;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13.Tests
{
    [TestClass()]
    public class Day13Tests
    {
        [TestMethod()]
        public void GetMirrorCountTest()
        {
            var result = MirrorFinder.GetMirrorCount("Day13TestInput.txt", false);
            Assert.AreEqual(405,result);
        }
        [TestMethod()]
        public void GetMirrorCountTestFullData()
        {
            var result = MirrorFinder.GetMirrorCount("Day13Input.txt", false);
            Assert.AreEqual(37113, result);
        }

        [TestMethod()]
        public void GetMirrorCountTestSmudge()
        {
            var result = MirrorFinder.GetMirrorCount("Day13TestInput.txt", true);
            Assert.AreEqual(400, result);
        }
        [TestMethod()]
        public void GetMirrorCountTestSmudgeFullData()
        {
            var result = MirrorFinder.GetMirrorCount("Day13Input.txt", true);
            Assert.AreEqual(30449, result);
        }
    }
}