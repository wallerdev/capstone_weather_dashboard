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
        public void TestZoneWithUnknownZipCode()
        {
            var zips = new[] { "01810", "01812", "01830", "01831", "01832", "01833", "01834", "01835", "01840", "01841", "01842", "01843", "01844", "01845", "01860", "01885", "01899", "01901", "01902", "01903", "01904", "01905", "01906", "01907", "01908", "01910", "01913", "01915", "01921", "01922", "01923", "01929", "01930", "01931", "01936", "01937", "01938", "01940", "01944", "01945", "01949", "01950", "01951", "01952", "01960", "01961", "01965", "01966", "01969", "01970", "01971", "01982", "01983", "01984", "01985", "05501", "05544" };
            Assert.IsTrue(_zoneLookup.GetZipCodes("MAZ006").Select(z => z.Code).SequenceEqual(zips));
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

        [TestMethod]
        public void TestZoneRangeStart()
        {
            Assert.IsTrue(_zoneLookup.IsZone("MIZ068>070 - 075>076 - 082"));
        }
    }
}
