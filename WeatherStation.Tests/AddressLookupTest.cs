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
        private readonly AddressLookup _addressLookup = new AddressLookup();
        private readonly Address _address = new Address(null, null, null, "48823");

        [TestMethod]
        public void TestCountyLookupFromZipCode()
        {
            Assert.AreEqual("Ingham", _addressLookup.GetAddressFromZipCode(_address.ZipCode).County);
        }

        [TestMethod]
        public void TestStateLookupFromZipCode()
        {
            Assert.AreEqual(new State("MI"), _addressLookup.GetAddressFromZipCode(_address.ZipCode).State);
        }

        [TestMethod]
        public void TestCityLookupFromZipCode()
        {
            Assert.AreEqual("East Lansing", _addressLookup.GetAddressFromZipCode(_address.ZipCode).City);
        }

        [TestMethod]
        public void TestGeocodeLookupFromZipCode()
        {
            double accuracy = 0.25;
            var address = _addressLookup.GetAddressFromZipCode(_address.ZipCode);
            Assert.IsTrue(Math.Abs(42.7369792 - address.Geocode.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - address.Geocode.Longitude) < accuracy);
        }

        [TestMethod]
        public void TestZipCodeLookupFromZipCode()
        {
            Assert.AreEqual("48823", _addressLookup.GetAddressFromZipCode(_address.ZipCode).ZipCode);
        }

        [TestMethod]
        public void TestCountyLookupFromCity()
        {
            Assert.AreEqual("Ingham", _addressLookup.GetAddressFromCity(_address.City).County);
        }

        [TestMethod]
        public void TestStateLookupFromCity()
        {
            Assert.AreEqual(new State("MI"), _addressLookup.GetAddressFromCity(_address.City).State);
        }

        [TestMethod]
        public void TestCityLookupFromCity()
        {
            Assert.AreEqual("East Lansing", _addressLookup.GetAddressFromCity(_address.City).City);
        }

        [TestMethod]
        public void TestGeocodeLookupFromCity()
        {
            double accuracy = 0.25;
            var address = _addressLookup.GetAddressFromCity(_address.City);
            Assert.IsTrue(Math.Abs(42.7369792 - address.Geocode.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - address.Geocode.Longitude) < accuracy);
        }
    }
}
