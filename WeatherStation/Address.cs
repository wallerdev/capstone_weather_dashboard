using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using WeatherStation.Geocode;
using System;

namespace WeatherStation
{
    public class Address
    {
        private static readonly WebClient _webClient = new WebClient();
        private static readonly GoogleGeocoder _geocoder = new GoogleGeocoder();
        private static ZoneLookup _zoneLookup = new ZoneLookup();
        private static ZipCodeLookup _zipCodeLookup = new ZipCodeLookup();

        public string FullAddress
        {
            get
            {
                string address = "";
                if (!string.IsNullOrEmpty(StreetAddress))
                {
                    address += StreetAddress + " ";
                }
                if (!string.IsNullOrEmpty(City))
                {
                    address += City + " ";
                }
                if (State != null)
                {
                    address += State.Abbreviation + " ";
                }
                if (!string.IsNullOrEmpty(ZipCode))
                {
                    address += ZipCode;
                }
                return address;
            }
        }

        public string StreetAddress
        {
            get;
            private set;
        }

        public string City
        {
            get;
            private set;
        }

        public State State
        {
            get;
            private set;
        }

        public string ZipCode
        {
            get;
            private set;
        }

        public string County
        {
            get;
            private set;
        }

        public double Latitude
        {
            get;
            private set;
        }

        public double Longitude
        {
            get;
            private set;
        }

        public Address(string streetAddress, string city, string state, string zipCode)
            : this(streetAddress, city, state, zipCode, null, 0.0, 0.0)
        {
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county)
            : this(streetAddress, city, state, zipCode, county, 0.0, 0.0)
        {
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county, double latitude, double longitude)
        {
            StreetAddress = streetAddress;
            City = city;
            if (!string.IsNullOrEmpty(state))
            {
                State = new State(state);
            }
            ZipCode = zipCode;
            County = county;
            Latitude = latitude;
            Longitude = longitude;
        }

        public static IEnumerable<Address> Search(string searchAddress)
        {
            var addresses = new List<Address>();

            if (_zoneLookup.IsZone(searchAddress))
            {
                foreach (var zip in _zoneLookup.GetZipCodes(searchAddress))
                {
                    addresses.Add(FromZipCode(zip));
                }
            }
            else if (_zipCodeLookup.IsZipCode(searchAddress))
            {
                addresses.Add(_zipCodeLookup.GetAddress(searchAddress));
            }
            else if(_zipCodeLookup.IsCityAndState(searchAddress))
            {
                addresses.Add(_zipCodeLookup.GetAddressFromCityAndState(searchAddress));
            }
            else
            {
                var response = _geocoder.Search(searchAddress);
                if (response != null)
                {
                    var address = new Address(response.Address, response.City, response.State, response.ZipCode, response.County, response.Latitude, response.Longitude);
                    addresses.Add(address);
                }
            }

            return addresses;
        }

        public static Address FromZipCode(string zipCode)
        {
            return _zipCodeLookup.GetAddress(zipCode);
        }

        /// <summary>
        /// Weather Underground historical data is based on airports. This function is provided to get the closest airport
        /// to a location using Weather Underground's API.
        /// </summary>
        /// <returns>Airport code of closest airport.</returns>
        public string FetchClosestAirportCode()
        {

            string apiUrl = string.Format("http://api.wunderground.com/auto/wui/geo/GeoLookupXML/index.xml?query={0}",
                                          FullAddress);
            string xml = _webClient.DownloadString(apiUrl);
            var document = XDocument.Parse(xml);
            var element = document.XPathSelectElement("/location/nearby_weather_stations/airport/station/icao");
            return element.Value;
        }
    }
}
