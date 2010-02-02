using System.Net;
using System.IO;

namespace WeatherStation
{
    /// <summary>
    /// Uses tinygeocoder to go from an address (full address, zip code, etc.) to 
    /// latitude and longitude necessary for NOAA requests
    /// </summary>
    public class Geocode
    {
        private const string GeocodeAddress = "http://tinygeocoder.com/create-api.php?";

        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }

        public Geocode(string address)
        {
            string requestUrl = GetGeocodeRequestUrl(address);
            var request = ((HttpWebRequest)WebRequest.Create(requestUrl));

            using (var response = request.GetResponse())
            {
                var geocodeStream = response.GetResponseStream();

                var reader = new StreamReader(geocodeStream);
                string latitudeAndLongitudeCsv = reader.ReadToEnd();

                // parse the csv and get the two data points
                string latitude = latitudeAndLongitudeCsv.Split(',')[0].Trim();
                string longitude = latitudeAndLongitudeCsv.Split(',')[1].Trim();

                Latitude = double.Parse(latitude);
                Longitude = double.Parse(longitude);
            }

        }

        private static string GetGeocodeRequestUrl(string address)
        {
            return string.Format("{0}q={1}", GeocodeAddress, address);
        }
    }
}