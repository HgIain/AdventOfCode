using Day1Point5;

namespace Day1Tests
{
    [TestClass]
    public class Day1Point5Tests
    {
        [TestMethod]
        public void TestMethod()
        {
            var result = Matcher.Process("testinput1point5.txt");

            Assert.AreEqual(281, result);
        }

        [TestMethod]
        public void TestMethodFullData()
        {
            var result = Matcher.Process("day1point5input.txt");

            Assert.AreEqual(55902,result);
        }
    }
}