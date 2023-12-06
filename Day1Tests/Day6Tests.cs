using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day6;

namespace UnitTests
{
    [TestClass]
    public class Day6Tests
    {
        [TestMethod]
        public void TestMulitple()
        {
            var boatTimer = new BoatTimer("Day6TestInput.txt");
            var result = boatTimer.Process(false);

            Assert.AreEqual(288, result);

        }

        [TestMethod]
        public void TestMulitpleFull()
        {
            var boatTimer = new BoatTimer("Day6Input.txt");
            var result = boatTimer.Process(false);

            Assert.AreEqual(608902, result);

        }

        [TestMethod]
        public void TestSingle()
        {
            var boatTimer = new BoatTimer("Day6TestInput.txt");
            var result = boatTimer.Process(true);

            Assert.AreEqual(71503, result);

        }

        [TestMethod]
        public void TestSingleFull()
        {
            var boatTimer = new BoatTimer("Day6Input.txt");
            var result = boatTimer.Process(true);

            Assert.AreEqual(46173809, result);

        }
    }
}
