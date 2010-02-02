using System.Collections.Generic;
using System;
using System.Linq;

namespace WeatherStation
{
    /// <summary>
    /// Looks up relavent information based on zipcode.
    /// Source: http://www.populardata.com/zipcode_database.html
    /// </summary>
    public class ZipCodeLookup
    {
        readonly IEnumerable<ZipCodeLookupEntry> _zipCodeLookupEntries;

        public ZipCodeLookup()
        {
            IEnumerable<string> entries = Properties.Resources.ZipCodeTable.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            _zipCodeLookupEntries = entries.Select(entry =>
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

        public string GetCity(string zipCode)
        {
            return GetEntry(zipCode).City;
        }

        ZipCodeLookupEntry GetEntry(string zipCode)
        {
            return _zipCodeLookupEntries.First(e => e.ZipCode == zipCode);
        }
    }
}


