using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for CountyListTest
    /// </summary>
    [TestClass]
    public class CountyListTest
    {
        private static readonly CountyList CountyList = new CountyList();

        [TestMethod]
        public void TestFindClosestCounty()
        {
            var address = Address.Search("East Lansing, MI").First();
            address.GeocodeAddress();

            // This might seem wrong but it is the closest county geocode
            Assert.AreEqual("Eaton", CountyList.FindClosestCounty(address.Geocode).Name);
        }

        [TestMethod]
        public void TestFindNearbyCounties()
        {
            var address = Address.Search("East Lansing, MI").First();
            address.GeocodeAddress();
            var counties = CountyList.FindNearbyCounties(address.Geocode, 25.0).Select(c => c.Name).ToList();
            counties.Sort();
            Assert.IsTrue(counties.SequenceEqual(new[] { "Eaton", "Ingham" }));
        }
    }
}
