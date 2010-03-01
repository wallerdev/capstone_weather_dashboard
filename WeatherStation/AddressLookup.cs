using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WeatherStation
{
    /// <summary>
    /// Looks up relavent information based on zipcode.
    /// Source: http://www.populardata.com/zipcode_database.html
    /// </summary>
    public class AddressLookup
    {
        private readonly Dictionary<string, Address> _zipCodeLookup = new Dictionary<string, Address>();
        private readonly Dictionary<KeyValuePair<string, State>, Address> _cityStateLookup = new Dictionary<KeyValuePair<string, State>, Address>();
        Regex cityCommaState = new Regex("\\s*(.+),\\s*(.+)\\s*");
        Regex citySpaceState = new Regex("\\s*(.+)\\s+(\\w{2})\\s*"); // Only supports two letter state codes for now

        public AddressLookup()
        {
            IEnumerable<string> entries = Properties.Resources.ZipCodeTable.Split(new[] { "\r\n" }, StringSplitOptions.None);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                var address = new Address(null, parts[3], parts[4], parts[0], parts[5], double.Parse(parts[1]),
                                          double.Parse(parts[2]));
                _cityStateLookup[new KeyValuePair<string, State>(parts[3], new State(parts[4]))] = address;
                _zipCodeLookup[address.ZipCode] = address;
            }
        }

        public Address GetAddressFromZipCode(string zipCode)
        {
            return _zipCodeLookup[zipCode];
        }

        public bool IsZipCode(string searchAddress)
        {
            return _zipCodeLookup.ContainsKey(searchAddress);
        }

        public bool IsCityAndState(string searchAddress)
        {
            return cityCommaState.IsMatch(searchAddress) || citySpaceState.IsMatch(searchAddress);
        }

        public Address GetAddressFromCityAndState(string searchAddress)
        {
            Match match;
            if ((match = cityCommaState.Match(searchAddress)).Success || (match = citySpaceState.Match(searchAddress)).Success)
            {
                var pair = new KeyValuePair<string, State>(match.Groups[1].Value, new State(match.Groups[2].Value));
                return _cityStateLookup[pair];
            }
            throw new NotImplementedException();
        }
    }
}


