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

        public GoogleGeocoderTest()
        {
            _geocoder = new GoogleGeocoder();
        }

        [TestMethod]
        public void TestLatitudeAndLongitude()
        {
            double accuracy = 0.25;
            var result = _geocoder.Geocode(new Address(null, null, null, "48823"));
            Assert.IsTrue(Math.Abs(42.7369792 - result.Geocode.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - result.Geocode.Longitude) < accuracy);
        }

        [TestMethod]
        public void TestAddressDetails()
        {
            var result = _geocoder.Geocode(new Address(null, null, null, "48823"));
            Assert.AreEqual("48823", result.ZipCode);
            Assert.AreEqual("East Lansing", result.City);
            Assert.AreEqual("Ingham", result.County);
            Assert.AreEqual(new State("MI"), result.State);
        }

        [TestMethod]
        public void TestSearch()
        {
            var result = _geocoder.Search("48823");
            Assert.AreEqual("48823", result.ZipCode);
            Assert.AreEqual("East Lansing", result.City);
            Assert.AreEqual("Ingham", result.County);
            Assert.AreEqual(new State("MI"), result.State);
        }

        [TestMethod]
        public void TestReverseGeocode()
        {
            var result = _geocoder.ReverseGeocode(42.7369792, -84.4838654);
            Assert.AreEqual("48823", result.ZipCode);
            Assert.AreEqual("East Lansing", result.City);
            Assert.AreEqual("Ingham", result.County);
            Assert.AreEqual(new State("MI"), result.State);
        }

        [TestMethod]
        public void TestInvalidAddressSearch()
        {
            var result = _geocoder.Search("THISISNOTAREALPLACETHATGOOGLESHOULDEVERFIND");
            Assert.IsNull(result);
        }
    }
}
