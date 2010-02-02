using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ZipCodeLookupTest
    {
        ZipCodeLookup zipCodeLookup;

        public ZipCodeLookupTest()
        {
            zipCodeLookup = new ZipCodeLookup();
        }

        [TestMethod]
        public void TestCountyLookup()
        {
            Assert.AreEqual("INGHAM", zipCodeLookup.GetCounty("48864"));
        }

        [TestMethod]
        public void TestStateLookup()
        {
            Assert.AreEqual("MI", zipCodeLookup.GetState("48864"));
        }
    }
}
