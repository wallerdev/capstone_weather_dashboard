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
            var addresses = Address.Search("North Leslie MI");
            Assert.AreEqual(1, addresses.Count());
            var address = addresses.Single();
            Assert.IsNull(address.StreetAddress);
            Assert.AreEqual("North Leslie", address.City);
            Assert.AreEqual("Ingham", address.County);
            Assert.AreEqual(new State("MI"), address.State);
            Assert.AreEqual(42.4888206, address.Latitude);
            Assert.AreEqual(-84.4281308, address.Longitude);
        }
    }
}
