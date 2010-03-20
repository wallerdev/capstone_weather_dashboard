using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for CityListTest
    /// </summary>
    [TestClass]
    public class CityListTest
    {
        private static readonly CityList CityList = new CityList();

        [TestMethod]
        public void TestFindClosestCity()
        {
            var address = Address.Search("East Lansing, MI").First();
            address.GeocodeAddress();

            // This might seem wrong but it is the closest county geocode
            Assert.AreEqual("East Lansing", CityList.FindClosestCity(address.Geocode).Name);
        }

        [TestMethod]
        public void TestFindNearbyCities()
        {
            var address = Address.Search("East Lansing, MI").First();
            address.GeocodeAddress();
            var cities = CityList.FindNearbyCities(address.Geocode, 5.0).Select(c => c.Name).ToList();
            cities.Sort();
            Assert.IsTrue(cities.SequenceEqual(new[] { "East Lansing", "Haslett", "Lansing", "Okemos" }));
        }

        [TestMethod]
        public void TestGetCity()
        {
            var city = CityList.GetCity("East Lansing", new State("MI"));
            Assert.AreEqual("East Lansing", city.Name);
            Assert.AreEqual(new State("MI"), city.State);

            const double accuracy = 0.01;
            Assert.IsTrue(Math.Abs(42.7369792 - city.Geocode.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4838654 - city.Geocode.Longitude) < accuracy);
        }
    }
}
