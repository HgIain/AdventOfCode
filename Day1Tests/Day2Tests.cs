using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2.Tests
{
    [TestClass()]
    public class Day2Tests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var result = Matcher.Process("day2testinput.txt");
            Assert.AreEqual(8, result);
        }

        public void ProcessTestFullData()
        {
            var result = Matcher.Process("day2input.txt");
            Assert.AreEqual(2162, result);
        }
    }
}