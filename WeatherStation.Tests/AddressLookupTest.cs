using System;
using System.Linq;
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
        private readonly Address _address = Address.Search("48823").First();

        [TestMethod]
        public void TestCountyLookupFromZipCode()
        {
            Assert.AreEqual("Ingham", _addressLookup.GetAddressFromZipCode(_address.ZipCode).County.Name);
        }

        [TestMethod]
        public void TestStateLookupFromZipCode()
        {
            Assert.AreEqual("MI", _addressLookup.GetAddressFromZipCode(_address.ZipCode).State.Abbreviation);
        }

        [TestMethod]
        public void TestCityLookupFromZipCode()
        {
            Assert.AreEqual("East Lansing", _addressLookup.GetAddressFromZipCode(_address.ZipCode).City.Name);
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
            Assert.AreEqual("48823", _addressLookup.GetAddressFromZipCode(_address.ZipCode).ZipCode.Code);
        }

        [TestMethod]
        public void TestCountyLookupFromCity()
        {
            Assert.AreEqual("Ingham", _addressLookup.GetAddressFromCity(_address.City).County.Name);
        }

        [TestMethod]
        public void TestStateLookupFromCity()
        {
            Assert.AreEqual("MI", _addressLookup.GetAddressFromCity(_address.City).State.Abbreviation);
        }

        [TestMethod]
        public void TestCityLookupFromCity()
        {
            Assert.AreEqual("East Lansing", _addressLookup.GetAddressFromCity(_address.City).City.Name);
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
