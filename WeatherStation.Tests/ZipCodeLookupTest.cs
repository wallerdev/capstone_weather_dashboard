using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ZipCodeLookupTest
    {
        readonly ZipCodeLookup _zipCodeLookup;

        public ZipCodeLookupTest()
        {
            _zipCodeLookup = new ZipCodeLookup();
        }

        [TestMethod]
        public void TestCountyLookup()
        {
            Assert.AreEqual("INGHAM", _zipCodeLookup.GetCounty("48864"));
        }

        [TestMethod]
        public void TestStateLookup()
        {
            Assert.AreEqual("MI", _zipCodeLookup.GetState("48864"));
        }

        [TestMethod]
        public void TestCityLookup()
        {
            Assert.AreEqual("OKEMOS", _zipCodeLookup.GetCity("48864"));
        }
    }
}
