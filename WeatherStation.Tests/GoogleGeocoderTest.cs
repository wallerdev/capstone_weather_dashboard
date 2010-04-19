using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Tests the GoogleGeocoder class.
    /// </summary>
    [TestClass]
    public class GoogleGeocoderTest
    {
        private GoogleGeocoder _geocoder;
        private Address _eastLansingCityHall = new Address("410 Abbot Rd", "East Lansing", "MI", "48823", "Ingham");

        public GoogleGeocoderTest()
        {
            _geocoder = new GoogleGeocoder();
        }

        [TestMethod]
        public void TestLatitudeAndLongitude()
        {
            double accuracy = 0.25;
            var result = _geocoder.Geocode(_eastLansingCityHall);
            Assert.IsTrue(Math.Abs(42.7369792 - result.Geocode.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - result.Geocode.Longitude) < accuracy);
        }

        [TestMethod]
        public void TestAddressDetails()
        {
            var result = _geocoder.Geocode(_eastLansingCityHall);
            Assert.AreEqual("48823", result.ZipCode.Code);
            Assert.AreEqual("East Lansing", result.City.Name);
            Assert.AreEqual("Ingham", result.County.Name);
            Assert.AreEqual("410 Abbot Rd", result.StreetAddress);
            Assert.AreEqual("MI", result.State.Abbreviation);
        }

        [TestMethod]
        public void TestSearch()
        {
            var result = _geocoder.Search("48823");
            Assert.AreEqual("48823", result.ZipCode.Code);
            Assert.AreEqual("East Lansing", result.City.Name);
            Assert.AreEqual("Clinton", result.County.Name);
            Assert.AreEqual("MI", result.State.Abbreviation);
        }

        [TestMethod]
        public void TestReverseGeocode()
        {
            var result = _geocoder.ReverseGeocode(42.7369792, -84.4838654);
            Assert.AreEqual("48823", result.ZipCode.Code);
            Assert.AreEqual("East Lansing", result.City.Name);
            Assert.AreEqual("Ingham", result.County.Name);
            Assert.AreEqual("MI", result.State.Abbreviation);
        }

        [TestMethod]
        public void TestInvalidAddressSearch()
        {
            var result = _geocoder.Search("THISISNOTAREALPLACETHATGOOGLESHOULDEVERFIND");
            Assert.IsNull(result);
        }
    }
}
