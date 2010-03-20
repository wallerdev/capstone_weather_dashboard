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
        private static readonly Dictionary<string, Address> ZipCodeLookup = new Dictionary<string, Address>();
        private static readonly Dictionary<KeyValuePair<string, State>, Address> CityStateLookup = new Dictionary<KeyValuePair<string, State>, Address>();
        private static readonly Regex CityCommaState = new Regex("\\s*(.+),\\s*(.+)\\s*");
        private static readonly Regex CitySpaceState = new Regex("\\s*(.+)\\s+(\\w{2})\\s*"); // Only supports two letter state codes for now

        static AddressLookup()
        {
            IEnumerable<string> entries = Properties.Resources.ZipCodeTable.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                var address = new Address(null, parts[3], parts[4], parts[0], parts[5], new Geocode(
                    double.Parse(parts[1]), double.Parse(parts[2])));
                CityStateLookup[new KeyValuePair<string, State>(parts[3], new State(parts[4]))] = address;
                ZipCodeLookup[address.ZipCode] = address;
            }
        }

        public Address GetAddressFromZipCode(string zipCode)
        {
            return ZipCodeLookup[zipCode];
        }

        public bool IsZipCode(string searchAddress)
        {
            return ZipCodeLookup.ContainsKey(searchAddress);
        }

        public bool IsCityAndState(string searchAddress)
        {
            if (CityCommaState.IsMatch(searchAddress) || CitySpaceState.IsMatch(searchAddress))
            {
                Match match;
                if ((match = CityCommaState.Match(searchAddress)).Success || (match = CitySpaceState.Match(searchAddress)).Success)
                {
                    var pair = new KeyValuePair<string, State>(match.Groups[1].Value, new State(match.Groups[2].Value));
                    return CityStateLookup.ContainsKey(pair);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Address GetAddressFromCityAndState(string searchAddress)
        {
            Match match;
            if ((match = CityCommaState.Match(searchAddress)).Success || (match = CitySpaceState.Match(searchAddress)).Success)
            {
                var pair = new KeyValuePair<string, State>(match.Groups[1].Value, new State(match.Groups[2].Value));
                return CityStateLookup[pair];
            }
            throw new ArgumentException("Unknown city and state", "searchAddress");
        }
    }
}


