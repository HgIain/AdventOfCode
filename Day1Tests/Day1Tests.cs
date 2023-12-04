using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day1;

namespace UnitTests
{
    [TestClass]
    public class Day1Tests
    {
        [TestMethod]
        public void TestMethod()
        {
            var result = Matcher.Process("testinput1.txt");

            Assert.AreEqual(142, result);
        }

        [TestMethod]
        public void TestMethodFullData()
        {
            var result = Matcher.Process("day1input.txt");

            Assert.AreEqual(56465, result);
        }
    }
}
