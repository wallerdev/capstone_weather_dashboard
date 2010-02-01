using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WeatherStation
{
    /// <summary>
    /// Uses tinygeocoder to go from an address (full address, zip code, etc.) to 
    /// latitude and longitude necessary for NOAA requests
    /// </summary>
    class Geocode
    {
        private string _geocodeAddress = "http://tinygeocoder.com/create-api.php?";
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
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

            using (var response = request.GetResponse())
            {
                Stream geocodeStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(geocodeStream);
                string latitudeAndLongitudeCSV = reader.ReadToEnd();

                // parse the csv and get the two data points
                string latitude = latitudeAndLongitudeCSV.Split(',')[0].Trim();
                string longitude = latitudeAndLongitudeCSV.Split(',')[1].Trim();

                Latitude = double.Parse(latitude);
                Longitude = double.Parse(longitude);
            }

        }

        private string GetGeocodeRequestUrl(string address)
        {
            return string.Format("{0}q={1}", _geocodeAddress, address);
        }
    }
}