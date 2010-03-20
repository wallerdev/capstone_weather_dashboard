using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class CityList
    {
        private static readonly List<City> Cities = new List<City>();
        static CityList()
        {
            IEnumerable<string> entries = Properties.Resources.CityGeocodes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                // This is in the form longitude, latitude
                var city = new City(parts[0], new State(parts[1]), new Geocode(double.Parse(parts[3]), double.Parse(parts[2])));
                Cities.Add(city);
            }
        }

        public City FindClosestCity(Geocode geocode)
        {
            Cities.Sort((a, b) => a.Geocode.DistanceTo(geocode).CompareTo(b.Geocode.DistanceTo(geocode)));
            foreach(var city in Cities.Take(10))
            {
                Trace.WriteLine(string.Format("{0}, {1}", city.Name, city.Geocode.DistanceTo(geocode)));
            }
            return Cities.First();
        }

        public IEnumerable<City> FindNearbyCities(Geocode geocode, double rangeInMiles)
        {
            return Cities.Where(c => c.Geocode.DistanceTo(geocode) < rangeInMiles);
        }
    }
}
