using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Tests the AddressLookup class
    /// </summary>
    [TestClass]
    public class AddressLookupTest
    {
        readonly AddressLookup _addressLookup;

        public AddressLookupTest()
        {
            _addressLookup = new AddressLookup();
        }

        [TestMethod]
        public void TestCountyLookupFromZipCode()
        {
            Assert.AreEqual("Ingham", _addressLookup.GetAddressFromZipCode("48823").County);
        }

        [TestMethod]
        public void TestStateLookupFromZipCode()
        {
            Assert.AreEqual(new State("MI"), _addressLookup.GetAddressFromZipCode("48823").State);
        }

        [TestMethod]
        public void TestCityLookupFromZipCode()
        {
            Assert.AreEqual("East Lansing", _addressLookup.GetAddressFromZipCode("48823").City);
        }

        [TestMethod]
        public void TestGeocodeLookupFromZipCode()
        {
            double accuracy = 0.25;
            var address = _addressLookup.GetAddressFromZipCode("48823");
            Assert.IsTrue(Math.Abs(42.7369792 - address.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - address.Longitude) < accuracy);
        }

        [TestMethod]
        public void TestZipCodeLookupFromZipCode()
        {
            Assert.AreEqual("48823", _addressLookup.GetAddressFromZipCode("48823").ZipCode);
        }

        [TestMethod]
        public void TestCountyLookupFromCityAndState()
        {
            Assert.AreEqual("Ingham", _addressLookup.GetAddressFromCityAndState("East Lansing, MI").County);
        }

        [TestMethod]
        public void TestStateLookupFromCityAndState()
        {
            Assert.AreEqual(new State("MI"), _addressLookup.GetAddressFromCityAndState("East Lansing, MI").State);
        }

        [TestMethod]
        public void TestCityLookupFromCityAndState()
        {
            Assert.AreEqual("East Lansing", _addressLookup.GetAddressFromCityAndState("East Lansing, MI").City);
        }

        [TestMethod]
        public void TestGeocodeLookupFromCityAndState()
        {
            double accuracy = 0.25;
            var address = _addressLookup.GetAddressFromZipCode("48823");
            Assert.IsTrue(Math.Abs(42.7369792 - address.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - address.Longitude) < accuracy);
        }
    }
}
