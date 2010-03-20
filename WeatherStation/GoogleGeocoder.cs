using System;
using System.Net;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;

namespace WeatherStation
{
    /// <summary>
    /// Uses Google to go from an address (full address, zip code, etc.) to 
    /// latitude and longitude
    /// </summary>
    public class GoogleGeocoder
    {
        private const string ApiKey = "ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w";
        private const string GeocodeAddress = "http://maps.google.com/maps/geo?q={0}&output=xml&sensor=false&gl=us&key={1}";
        private const string Success = "200";
        private const string NoLocationFound = "602";
        private WebClient _client = new WebClient();

        public Address Geocode(Address address)
        {
            string searchAddress = address.FullAddress;
            if (string.IsNullOrEmpty(address.StreetAddress) && !string.IsNullOrEmpty(address.ZipCode))
            {
                searchAddress = address.ZipCode;
            }
            return Search(searchAddress);
        }

        public Address Search(string address)
        {
            string escapedAddress = Uri.EscapeDataString(address);
            string response = _client.DownloadString(GetGeocodeRequestUrl(escapedAddress));
            return ParseGoogleGeocoderResponse(XDocument.Parse(response));
        }

        public Address ReverseGeocode(double latitude, double longitude)
        {
            string requestUrl = GetReverseGeocodeRequestUrl(latitude, longitude);
            string response = _client.DownloadString(requestUrl);
            return ParseGoogleGeocoderResponse(XDocument.Parse(response));
        }

        private static Address ParseGoogleGeocoderResponse(XDocument doc)
        {
            XNamespace earthNamespace = "http://earth.google.com/kml/2.0";
            XNamespace addressNamespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0";
            var response = new Address();
            var geocodeSetter = new Dictionary<string, Action<string>>
            {
                {"AdministrativeAreaName", value => response.State = new State(value)},
                {"SubAdministrativeAreaName", value => response.County = value},
                {"LocalityName", value => response.City = value},
                {"ThoroughfareName", value => response.StreetAddress = value},
                {"PostalCodeNumber", value => response.ZipCode = value}
            };

            var code = doc.Descendants(earthNamespace + "code").Single().Value;
            if(code != Success)
            {
                if(code == NoLocationFound)
                {
                    return null;
                }
                throw new InvalidOperationException("Google failed to geocode request: code " + code);
            }

            var placemark = doc.Descendants(earthNamespace + "Placemark").First();

            foreach (var pair in geocodeSetter)
            {
                var element = placemark.Descendants(addressNamespace + pair.Key).SingleOrDefault();
                if (element != null)
                {
                    pair.Value(element.Value);
                }
            }

            var latitudeAndLongitude = placemark.Descendants(earthNamespace + "LatLonBox").Single();
            response.Geocode = new Geocode(
                double.Parse(latitudeAndLongitude.Attribute("north").Value),
                double.Parse(latitudeAndLongitude.Attribute("east").Value));

            return response;
        }

        private static string GetGeocodeRequestUrl(string query)
        {
            return string.Format(GeocodeAddress, query, ApiKey);
        }

        private static string GetReverseGeocodeRequestUrl(double latitude, double longitude)
        {
            string query = string.Format("{0},{1}", latitude, longitude);
            return GetGeocodeRequestUrl(query);
        }
    }
}