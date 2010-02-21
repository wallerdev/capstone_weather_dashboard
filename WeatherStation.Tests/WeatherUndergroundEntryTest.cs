using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherStation.WeatherEventProviders;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for WeatherUndergroundEntryTest
    /// </summary>
    [TestClass]
    public class WeatherUndergroundEntryTest
    {
        [TestMethod]
        public void TestCalmWindSpeed()
        {
            Assert.AreEqual(
                new WeatherUndergroundEntry(DateTime.Now, "12:16 AM,15.8,8.6,73,30.24,10.0,Calm,Calm,-,N/A,,Clear").
                    WindSpeed, 0);
        }

        [TestMethod]
        public void TestNotAvailableHumidity()
        {
            Assert.AreEqual(
                new WeatherUndergroundEntry(DateTime.Now, "5:53 PM,89.1,-9999,N/A,29.97,10.0,WSW,17.3,21.9,N/A,,Mostly Cloudy").
                    Humidity, null);
        }
    }
}
