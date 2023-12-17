using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day17;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17.Tests
{
    [TestClass()]
    public class Day17Tests
    {
        [TestMethod()]
        public void GetShortestRouteTest()
        {
            var heatLossRoute = new HeatLossRoute("Day17TestInput.txt");
            var result = heatLossRoute.GetShortestRoute();
            Assert.AreEqual(102, result);
        }

        [TestMethod()]
        public void GetShortestRouteUltraTest()
        {
            var heatLossRoute = new HeatLossRoute("Day17TestInput.txt",4,10);
            var result = heatLossRoute.GetShortestRoute();
            Assert.AreEqual(94, result);
        }

#if false
        [TestMethod()]
        public void GetShortestRouteUltraTest2()
        {
            var heatLossRoute = new HeatLossRoute("Day17TestInput2.txt", 4, 10);
            var result = heatLossRoute.GetShortestRoute();
            Assert.AreEqual(71, result);
        }
#endif
    }
}