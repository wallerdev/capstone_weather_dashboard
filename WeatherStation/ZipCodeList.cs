using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class ZipCodeList
    {
        private static readonly Dictionary<string, ZipCode> ZipCodeLookup = new Dictionary<string, ZipCode>();
        private static readonly List<Address> Addresses = new List<Address>();

        static ZipCodeList()
        {
            IEnumerable<string> entries = Properties.Resources.ZipCodeTable.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();

                // Make sure we create the zip code before the address because the address will try to look up
                // the zip code
                var zipCode = new ZipCode(parts[0], new Geocode(double.Parse(parts[1]), double.Parse(parts[2])));
                ZipCodeLookup[zipCode.Code] = zipCode;

                var address = new Address(null, parts[3], parts[4], parts[0], parts[5]);
                Addresses.Add(address);
            }
        }

        public static ZipCode GetZipCode(string zipCode)
        {
            return ZipCodeLookup[zipCode];
        }

        public static bool IsZipCode(string zipCode)
        {
            return ZipCodeLookup.ContainsKey(zipCode);
        }

        public static IEnumerable<Address> GetZipCodeAddresses()
        {
            return Addresses;
        }
    }
}
