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
        private static readonly Dictionary<ZipCode, Address> ZipCodeLookup = new Dictionary<ZipCode, Address>();
        private static readonly Dictionary<City, Address> CityStateLookup = new Dictionary<City, Address>();

        static AddressLookup()
        {
            foreach (var address in ZipCodeList.GetZipCodeAddresses())
            {
                ZipCodeLookup[address.ZipCode] = address;
                CityStateLookup[address.City] = address;
            }
        }


        public Address GetAddressFromZipCode(ZipCode zipCode)
        {
            var address = ZipCodeLookup[zipCode];
            return new Address(address.StreetAddress, address.City.Name, address.State.Name, 
                address.ZipCode.Code, address.County.Name, address.ZipCode.Geocode);
        }

        public Address GetAddressFromCity(City city)
        {
            var address = CityStateLookup[city];
            return new Address(address.StreetAddress, address.City.Name, address.State.Name,
                address.ZipCode.Code, address.County.Name, address.City.Geocode);
        }
    }
}


