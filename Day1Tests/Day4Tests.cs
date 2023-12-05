using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class Day4Tests
    {
        [TestMethod]
        public void TestScratchcards()
        {
            var result = Day4.Scratchcards.Process("Day4TestInput.txt");

            Assert.AreEqual(13, result);
        }

        [TestMethod]
        public void TestScratchcardsFulldata()
        {
            var result = Day4.Scratchcards.Process("Day4Input.txt");

            Assert.AreEqual(26443, result);
        }

        [TestMethod]
        public void TestScratchcardsV2()
        {
            var scratchcards = new Day4.ScratchcardsV2("Day4TestInput.txt");
            var result = scratchcards.Process();

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void TestScratchcardsV2FullData()
        {
            var scratchcards = new Day4.ScratchcardsV2("Day4Input.txt");
            var result = scratchcards.Process();

            Assert.AreEqual(6284877, result);
        }
    }
}
