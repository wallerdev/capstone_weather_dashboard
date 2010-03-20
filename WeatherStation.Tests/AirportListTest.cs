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
        private static AirportList _airportList = new AirportList();

        [TestMethod]
        public void TestFindClosestAirport()
        {
            var address = new Address(null, "East Lansing", "MI", "48823");
            address.GeocodeAddress();
            Assert.AreEqual("KLAN",_airportList.FindClosestAirport(address.Geocode).AirportCode);
        }
    }
}
