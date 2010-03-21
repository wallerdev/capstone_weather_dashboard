using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for ZoneLookupTest
    /// </summary>
    [TestClass]
    public class ZoneLookupTest
    {
        private ZoneLookup _zoneLookup = new ZoneLookup();

        [TestMethod]
        public void TestValidSimpleZone()
        {
            string zone = "MIZ001";
            Assert.IsTrue(_zoneLookup.IsZone(zone));
            Assert.IsTrue(_zoneLookup.GetZipCodes(zone).Select(z => z.Code).SequenceEqual(new[] { "49805", "49901", "49918", "49950" }));
        }

        [TestMethod]
        public void TestInvalidZone()
        {
            Assert.IsFalse(_zoneLookup.IsZone("ABC123"));
            Assert.IsFalse(_zoneLookup.IsZone("East Lansing, MI"));
        }

        [TestMethod]
        public void TestMultipleZones()
        {
            var miz001 = new[] { "49805", "49901", "49918", "49950" };
            var miz004 = new[] { "49908", "49919", "49946", "49962", "49970" };

            Assert.IsTrue(_zoneLookup.GetZipCodes("MIZ001 - 004").Select(z => z.Code).SequenceEqual(miz001.Concat(miz004)));
        }

        [TestMethod]
        public void TestZoneRange()
        {
            var miz001 = new[] { "49805", "49901", "49918", "49950" };
            var miz002 = new[] { "49910", "49912", "49925", "49929", "49948", "49953", "49960", "49967", "49971" };
            var miz003 = new[]
                             {
                                 "49905", "49913", "49916", "49917", "49921", "49922", "49930", "49931", "49934", "49942",
                                 "49945", "49952", "49955", "49958", "49961", "49963", "49965"
                             };
            var miz004 = new[] { "49908", "49919", "49946", "49962", "49970" };

            Assert.IsTrue(_zoneLookup.GetZipCodes("MIZ001>004").Select(z => z.Code).SequenceEqual(miz001.Concat(miz002).Concat(miz003).Concat(miz004)));
        }
    }
}
