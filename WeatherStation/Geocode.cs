using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class Geocode
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Geocode(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Uses Great Circle Distance Formula to find the distance between two geocodes in miles.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public double DistanceTo(Geocode geocode)
        {
            const double radiusOfEarthInMiles = 3958.76;

            double lat1 = Latitude * Math.PI / 180;
            double lat2 = geocode.Latitude * Math.PI / 180;
            double lon1 = Longitude * Math.PI / 180;
            double lon2 = geocode.Longitude * Math.PI / 180;

            double x = Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1);
            return radiusOfEarthInMiles * Math.Atan2(Math.Sqrt(1 - Math.Pow(x, 2)), x);
        }
    }
}
