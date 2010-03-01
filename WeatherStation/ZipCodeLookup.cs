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
        readonly Dictionary<string, Address> _addressLookup = new Dictionary<string, Address>();

        public ZipCodeLookup()
        {
            IEnumerable<string> entries = Properties.Resources.ZipCodeTable.Split(new[] { "\r\n" }, StringSplitOptions.None);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                var address = new Address(null, parts[3], parts[4], parts[0], parts[5], double.Parse(parts[1]),
                                          double.Parse(parts[2]));
                _addressLookup[address.ZipCode] = address;
            }
        }

        public Address GetAddress(string zipCode)
        {
            return _addressLookup[zipCode];
        }

        internal bool IsZipCode(string searchAddress)
        {
            return _addressLookup.ContainsKey(searchAddress);
        }

        internal bool IsCityAndState(string searchAddress)
        {
            throw new NotImplementedException();
        }

        internal Address GetAddressFromCityAndState(string searchAddress)
        {
            throw new NotImplementedException();
        }
    }
}


