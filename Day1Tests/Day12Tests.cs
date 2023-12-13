using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12.Tests
{
    [TestClass()]
    public class Day12Tests
    {
        [TestMethod()]
        public void GetVariantsTest()
        {
            var result = OperationalSpring.GetVariants("Day12TestInput.txt", 1);
            Assert.AreEqual(21, result);
        }
        [TestMethod()]
        public void GetVariantsTestFullData()
        {
            var result = OperationalSpring.GetVariants("Day12Input.txt", 1);
            Assert.AreEqual(7169, result);
        }

        [TestMethod()]
        public void GetExpandedVariantsTest()
        {
            var result = OperationalSpring.GetVariants("Day12TestInput.txt", 5);
            Assert.AreEqual(525152, result);
        }
#if false
        [TestMethod()]
        public void GetExpandedVariantsTestFullData()
        {
            var result = OperationalSpring.GetVariants("Day12Input.txt", 5);
            Assert.AreEqual(7169, result);
        }
#endif
    }
}