using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10.Tests
{
    [TestClass()]
    public class Day10Tests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var pipeFollower = new PipeFollower("Day10TestInput.txt");
            var result = pipeFollower.Process();
            Assert.AreEqual(4, result);
        }
        [TestMethod()]
        public void ProcessTest2()
        {
            var pipeFollower = new PipeFollower("Day10TestInput2.txt");
            var result = pipeFollower.Process();
            Assert.AreEqual(8, result);
        }
        [TestMethod()]
        public void ProcessTestFullData()
        {
            var pipeFollower = new PipeFollower("Day10Input.txt");
            var result = pipeFollower.Process();
            Assert.AreEqual(6842, result);
        }

        [TestMethod()]
        public void ProcessTestInside()
        {
            var pipeFollower = new PipeFollower("Day10TestInput3.txt");
            var result = pipeFollower.Process(true);
            Assert.AreEqual(4, result);
        }

        [TestMethod()]
        public void ProcessTestInside2()
        {
            var pipeFollower = new PipeFollower("Day10TestInput5.txt");
            var result = pipeFollower.Process(true);
            Assert.AreEqual(8, result);
        }
        [TestMethod()]
        public void ProcessTestInside3()
        {
            var pipeFollower = new PipeFollower("Day10TestInput6.txt");
            var result = pipeFollower.Process(true);
            Assert.AreEqual(10, result);
        }

        [TestMethod()]
        public void ProcessTestInsideFullData()
        {
            var pipeFollower = new PipeFollower("Day10Input.txt");
            var result = pipeFollower.Process(true);
            Assert.AreEqual(393, result);
        }

    }
}