using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20.Tests
{
    [TestClass()]
    public class Day20Tests
    {
        [TestMethod()]
        public void ProcessPulsesTest()
        {
            var pulseTracker = new PulseTracker("Day20TestInput.txt");
            var result = pulseTracker.ProcessPulses();
            Assert.AreEqual(32000000L,result);
        }
        [TestMethod()]
        public void ProcessPulsesTest2()
        {
            var pulseTracker = new PulseTracker("Day20TestInput2.txt");
            var result = pulseTracker.ProcessPulses();
            Assert.AreEqual(11687500L, result);
        }
        [TestMethod()]
        public void ProcessPulsesTestFullData()
        {
            var pulseTracker = new PulseTracker("Day20Input.txt");
            var result = pulseTracker.ProcessPulses();
            Assert.AreEqual(856482136L, result);
        }
        [TestMethod()]
        public void MinButtonsToRxTest()
        {
            var pulseTracker = new PulseTracker("Day20Input.txt");
            var result = pulseTracker.MinimumPulsesToRx();
            Assert.AreEqual(224046542165867L, result);
        }
    }
}