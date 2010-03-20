using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for AirportListTest
    /// </summary>
    [TestClass]
    public class AirportListTest
    {
        private static readonly AirportList AirportList = new AirportList();

        [TestMethod]
        public void TestFindClosestAirport()
        {
            var address = new Address(null, "East Lansing", "MI", "48823");
            address.GeocodeAddress();
            Assert.AreEqual("KLAN", AirportList.FindClosestAirport(address.Geocode).AirportCode);
        }

        [TestMethod]
        public void TestFindNearbyAirports()
        {
            var address = new Address(null, "East Lansing", "MI", "48823");
            address.GeocodeAddress();
            var airports = AirportList.FindNearbyAirports(address.Geocode, 50.0).Select(a => a.AirportCode).ToList();
            airports.Sort();
            Assert.IsTrue(airports.SequenceEqual(new [] { "KAMN", "KFNT", "KJXN", "KLAN"}));
        }
    }
}
