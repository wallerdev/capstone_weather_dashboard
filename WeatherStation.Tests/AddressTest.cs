using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for AddressTest
    /// </summary>
    [TestClass]
    public class AddressTest
    {
        [TestMethod]
        public void TestAddressSearch()
        {
            var addresses = Address.Search("Lansing MI");
            Assert.AreEqual(1, addresses.Count());
            var address = addresses.Single();
            Assert.IsNull(address.StreetAddress);
            Assert.AreEqual("Lansing", address.City.Name);
            Assert.AreEqual("Ingham", address.County.Name);
            Assert.AreEqual("MI", address.State.Abbreviation);

            double accuracy = 0.25;
            Assert.IsTrue(Math.Abs(42.7980673 - address.Geocode.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4274753 - address.Geocode.Longitude) < accuracy);
        }
    }
}
