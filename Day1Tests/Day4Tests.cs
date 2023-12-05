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
    }
}
