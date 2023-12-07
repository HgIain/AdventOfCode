using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7.Tests
{
    [TestClass()]
    public class Day7Tests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var camelCards = new CamelCards("Day7TestInput.txt");
            var result = camelCards.Process();
            Assert.AreEqual(6440, result);
        }
        [TestMethod()]
        public void ProcessTestFullData()
        {
            var camelCards = new CamelCards("Day7Input.txt");
            var result = camelCards.Process();
            Assert.AreEqual(252656917, result);
        }
        [TestMethod()]
        public void ProcessTestJokers()
        {
            var camelCards = new CamelCardsJoker("Day7TestInput.txt");
            var result = camelCards.Process();
            Assert.AreEqual(5905, result);
        }
        [TestMethod()]
        public void ProcessTestJokersFullData()
        {
            var camelCards = new CamelCardsJoker("Day7Input.txt");
            var result = camelCards.Process();
            Assert.AreEqual(253499763, result);
        }
    }
}