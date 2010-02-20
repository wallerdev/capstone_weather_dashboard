using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WeatherStation
{
    public class Address
    {
        static ZipCodeLookup zipCodeLookup = new ZipCodeLookup();
        private WebClient _webClient = new WebClient();

        public string FullAddress
        {
            get
            {
                return string.Format("{0} {1}, {2} {3}", StreetAddress, City, State.Abbreviation, ZipCode);
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
            get
            {
                // TODO: Make this dynamic
                return "48823";
            }
            set
            {
                
            }
        }

        public string County
        {
            get
            {
                // TODO: Make this dynamic
                return "Ingham";
            }
        }

        public Address(string streetAddress, string city, State state, string zipCode)
        {
            StreetAddress = streetAddress;
            City = city;
            State = state;
            ZipCode = zipCode;

        }

        public Address(string streetAddress, string city, string state, string zipCode)
            : this(streetAddress, city, new State(state), zipCode)
        {
        }

        public Address(string zipCode)
            : this(string.Empty, zipCodeLookup.GetCity(zipCode), zipCodeLookup.GetState(zipCode), zipCode)
        {
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
