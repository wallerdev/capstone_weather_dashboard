using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using WeatherStation.Geocode;
using System;

namespace WeatherStation
{
    public class Address
    {
        private WebClient _webClient = new WebClient();
        private GoogleGeocoder _geocoder = new GoogleGeocoder();

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
        {
            StreetAddress = streetAddress;
            City = city;
            if (!string.IsNullOrEmpty(state))
            {
                State = new State(state);
            }
            ZipCode = zipCode;

            var response = _geocoder.Geocode(this);
            StreetAddress = response.Address;
            City = response.City;
            State = new State(response.State);
            ZipCode = response.ZipCode;
            County = response.County;
            Latitude = response.Latitude;
            Longitude = response.Longitude;
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
