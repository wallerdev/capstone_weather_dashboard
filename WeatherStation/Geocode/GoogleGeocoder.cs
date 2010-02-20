using System;
using System.Net;
using System.Xml;

namespace WeatherStation.Geocode
{
    /// <summary>
    /// Uses tinygeocoder to go from an address (full address, zip code, etc.) to 
    /// latitude and longitude necessary for NOAA requests
    /// </summary>
    public class GoogleGeocoder
    {
        private static readonly string ApiKey = "ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w";
        private const string GeocodeAddress = "http://maps.google.com/maps/geo?q={0}&output=xml&sensor=false&key={1}";

        public static GoogleGeocodeResponse Geocode(string address)
        {
            address = Uri.EscapeDataString(address);
            string requestUrl = GetGeocodeRequestUrl(address);
            var request = ((HttpWebRequest)WebRequest.Create(requestUrl));

            using (var response = request.GetResponse())
            {
                var geocodeStream = response.GetResponseStream();
                using(var reader = new XmlTextReader(geocodeStream))
                {
                    reader.Namespaces = false;
                    var doc = new XmlDocument();
                    doc.Load(reader);
                    return ParseGoogleGeocoderResponse(doc);
                }
            }
        }

        public static GoogleGeocodeResponse ReverseGeocode(double latitude, double longitude)
        {
            string requestUrl = GetReverseGeocodeRequestUrl(latitude, longitude);
            var request = ((HttpWebRequest)WebRequest.Create(requestUrl));

            using (var response = request.GetResponse())
            {
                var geocodeStream = response.GetResponseStream();
                using (var reader = new XmlTextReader(geocodeStream))
                {
                    reader.Namespaces = false;
                    var doc = new XmlDocument();
                    doc.Load(reader);
                    return ParseGoogleGeocoderResponse(doc);
                }
            }
        }

        private static GoogleGeocodeResponse ParseGoogleGeocoderResponse(XmlDocument doc)
        {
            var placemark = doc.SelectSingleNode("//Placemark");

            var response = new GoogleGeocodeResponse();

            var countryNameCode = placemark.SelectSingleNode("AddressDetails/Country/CountryNameCode");
            if (countryNameCode != null)
            {
                response.CountryNameCode = countryNameCode.Value;
            }

            var countryName = placemark.SelectSingleNode("AddressDetails/Country/CountryName");
            if (countryNameCode != null)
            {
                response.CountryName = countryName.Value;
            }

            var state = placemark.SelectSingleNode("AddressDetails/Country/AdministrativeArea/AdministrativeAreaName");
            if (state != null)
            {
                response.State = state.Value;
            }

            var county = placemark.SelectSingleNode("AddressDetails/Country/AdministrativeArea/SubAdministrativeArea/SubAdministrativeAreaName");
            if (county != null)
            {
                response.County = county.Value;
            }

            var city = placemark.SelectSingleNode("AddressDetails/Country/AdministrativeArea/SubAdministrativeArea/Locality/LocalityName");
            if (city != null)
            {
                response.City = city.Value;
            }

            var address = placemark.SelectSingleNode("AddressDetails/Country/AdministrativeArea/SubAdministrativeArea/Locality/Thoroughfare/ThoroughfareName");
            if (address != null)
            {
                response.Address = address.Value;
            }

            var zipCode = placemark.SelectSingleNode("AddressDetails/Country/AdministrativeArea/SubAdministrativeArea/Locality/PostalCode/PostalCodeNumber");
            if(zipCode != null)
            {
                response.ZipCode = zipCode.Value;
            }

            var coordinates = placemark.SelectSingleNode("Point/coordinates");
            if (coordinates != null)
            {
                response.Latitude = double.Parse(coordinates.InnerText.Split(',')[0]);
                response.Longitude = double.Parse(coordinates.InnerText.Split(',')[1]);
            }

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