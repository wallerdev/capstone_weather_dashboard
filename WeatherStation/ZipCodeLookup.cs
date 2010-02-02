using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using System.Linq;

namespace WeatherStation
{
    /// <summary>
    /// Looks up relavent information based on zipcode.
    /// Source: http://www.populardata.com/zipcode_database.html
    /// </summary>
    public class ZipCodeLookup
    {
        IEnumerable<ZipCodeLookupEntry> zipCodeLookupEntries;
        public ZipCodeLookup()
        {
            IEnumerable<string> entries = Properties.Resources.ZipCodeTable.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            zipCodeLookupEntries = entries.Select(entry =>
                new ZipCodeLookupEntry(entry.Split(',').Select(data => data.Trim('"')).ToList()));
        }

        public string GetCounty(string zipCode)
        {
            return GetEntry(zipCode).County;
        }

        public string GetState(string zipCode)
        {
            return GetEntry(zipCode).State;
        }

        ZipCodeLookupEntry GetEntry(string zipCode)
        {
            return zipCodeLookupEntries.First(e => e.ZipCode == zipCode);
        }
    }
}


