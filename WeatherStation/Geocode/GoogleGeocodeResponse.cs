using System;

namespace WeatherStation.Geocode
{
    public class GoogleGeocodeResponse
    {
        public string Address
        {
            get;
            set;
        }

        public string CountryNameCode
        {
            get;
            set;
        }

        public string CountryName
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string County
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string ZipCode
        {
            get;
            set;
        }

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

        /// <summary>
        /// Uses Great Circle Distance Formula to find the distance between two geocodes in miles.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public double DistanceTo(GoogleGeocodeResponse response)
        {
            const double radiusOfEarthInMiles = 3963.0;

            double lat1 = Latitude * 180 / Math.PI;
            double lat2 = response.Latitude * 180 / Math.PI;
            double lon1 = Longitude * 180 / Math.PI;
            double lon2 = response.Longitude * 180 / Math.PI;

            double x = Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1);
            return radiusOfEarthInMiles * Math.Atan2(Math.Sqrt(1 - Math.Pow(x, 2)), x);
        }
    }
}
