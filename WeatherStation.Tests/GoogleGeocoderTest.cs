﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherStation.Geocode;

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
            var result = _geocoder.Geocode(new Address(null, null, null, "48823"));
            Assert.IsTrue(42.7369792 - result.Latitude < double.Epsilon);
            Assert.IsTrue(-84.4838654 - result.Longitude < double.Epsilon);
        }

        [TestMethod]
        public void TestAddressDetails()
        {
            var result = _geocoder.Geocode(new Address(null, null, null, "48823"));
            Assert.AreEqual("48823", result.ZipCode);
            Assert.AreEqual("East Lansing", result.City);
            Assert.AreEqual("Ingham", result.County);
            Assert.AreEqual("MI", result.State);
            Assert.AreEqual("US", result.CountryNameCode);
            Assert.AreEqual("USA", result.CountryName);
        }

        [TestMethod]
        public void TestReverseGeocode()
        {
            var result = _geocoder.ReverseGeocode(42.7369792, -84.4838654);
            Assert.AreEqual("48823", result.ZipCode);
            Assert.AreEqual("East Lansing", result.City);
            Assert.AreEqual("Ingham", result.County);
            Assert.AreEqual("MI", result.State);
            Assert.AreEqual("US", result.CountryNameCode);
            Assert.AreEqual("USA", result.CountryName);
        }
    }
}
