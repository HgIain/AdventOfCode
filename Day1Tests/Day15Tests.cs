using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day15;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15.Tests
{
    [TestClass()]
    public class Day15Tests
    {
        [TestMethod()]
        public void GenerateHashTest()
        {
            var result = InstructionHasher.GenerateHash("Day15TestInput.txt", false);
            Assert.AreEqual(1320, result);
        }
        [TestMethod()]
        public void GenerateHashTestFullData()
        {
            var result = InstructionHasher.GenerateHash("Day15Input.txt", false);
            Assert.AreEqual(511257, result);
        }

        [TestMethod()]
        public void GenerateHashTestBoxes()
        {
            var result = InstructionHasher.GenerateHash("Day15TestInput.txt", true);
            Assert.AreEqual(145, result);
        }
        [TestMethod()]
        public void GenerateHashTestBoxesFullData()
        {
            var result = InstructionHasher.GenerateHash("Day15Input.txt", true);
            Assert.AreEqual(239484, result);
        }
    }
}