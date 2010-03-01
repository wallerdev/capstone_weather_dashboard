using System;
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
            Assert.AreEqual("Ingham", _zipCodeLookup.GetAddress("48823").County);
        }

        [TestMethod]
        public void TestStateLookup()
        {
            Assert.AreEqual(new State("MI"), _zipCodeLookup.GetAddress("48823").State);
        }

        [TestMethod]
        public void TestCityLookup()
        {
            Assert.AreEqual("East Lansing", _zipCodeLookup.GetAddress("48823").City);
        }

        [TestMethod]
        public void TestGeocodeLookup()
        {
            double accuracy = 0.25;
            var address = _zipCodeLookup.GetAddress("48823");
            Assert.IsTrue(Math.Abs(42.7369792 - address.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - address.Longitude) < accuracy);
        }
    }
}
