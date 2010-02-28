using System;
using System.Net;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;

namespace WeatherStation.Geocode
{
    /// <summary>
    /// Uses Google to go from an address (full address, zip code, etc.) to 
    /// latitude and longitude
    /// </summary>
    public class GoogleGeocoder
    {
        private const string ApiKey = "ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w";
        private const string GeocodeAddress = "http://maps.google.com/maps/geo?q={0}&output=xml&sensor=false&key={1}";
        private WebClient _client = new WebClient();

        public GoogleGeocodeResponse Geocode(Address address)
        {
            string searchAddress = address.FullAddress;
            if (string.IsNullOrEmpty(address.StreetAddress) && !string.IsNullOrEmpty(address.ZipCode))
            {
                searchAddress = address.ZipCode;
            }
            return Search(searchAddress);
        }

        public GoogleGeocodeResponse Search(string address)
        {
            string escapedAddress = Uri.EscapeDataString(address);
            string response = _client.DownloadString(GetGeocodeRequestUrl(escapedAddress));
            return ParseGoogleGeocoderResponse(XDocument.Parse(response));
        }

        public GoogleGeocodeResponse ReverseGeocode(double latitude, double longitude)
        {
            string requestUrl = GetReverseGeocodeRequestUrl(latitude, longitude);
            string response = _client.DownloadString(requestUrl);
            return ParseGoogleGeocoderResponse(XDocument.Parse(response));
        }

        private static GoogleGeocodeResponse ParseGoogleGeocoderResponse(XDocument doc)
        {
            XNamespace earthNamespace = "http://earth.google.com/kml/2.0";
            XNamespace addressNamespace = "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0";
            var response = new GoogleGeocodeResponse();
            var geocodeSetter = new Dictionary<string, Action<string>>()
            {
                {"CountryNameCode", value => response.CountryNameCode = value},
                {"CountryName", value => response.CountryName = value},
                {"AdministrativeAreaName", value => response.State = value},
                {"SubAdministrativeAreaName", value => response.County = value},
                {"LocalityName", value => response.City = value},
                {"ThoroughfareName", value => response.Address = value},
                {"PostalCodeNumber", value => response.ZipCode = value}
            };

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
            response.Latitude = double.Parse(latitudeAndLongitude.Attribute("north").Value);
            response.Longitude = double.Parse(latitudeAndLongitude.Attribute("east").Value);

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