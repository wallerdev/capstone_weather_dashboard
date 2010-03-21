using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WeatherStation
{
    public class CityList
    {
        private static readonly List<City> Cities = new List<City>();
        private static readonly Regex CityCommaState = new Regex("\\s*(.+),\\s*(.+)\\s*");
        private static readonly Regex CitySpaceState = new Regex("\\s*(.+)\\s+(\\w{2})\\s*"); // Only supports two letter state codes for now

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

        public City GetCity(string city, State state)
        {
            return Cities.Single(c => c.Name == city && c.State.Equals(state));
        }

        public City GetCity(string cityAndState)
        {
            Match match;
            if ((match = CityCommaState.Match(cityAndState)).Success || (match = CitySpaceState.Match(cityAndState)).Success)
            {
                return GetCity(match.Groups[1].Value, new State(match.Groups[2].Value));
            }
            throw new ArgumentException("Unknown city and state", "cityAndState");
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

        public bool IsCity(string city, State state)
        {
            return Cities.Exists(c => c.Name == city && c.State.Equals(state));
        }

        public bool IsCityAndState(string cityAndState)
        {
            if (CityCommaState.IsMatch(cityAndState) || CitySpaceState.IsMatch(cityAndState))
            {
                Match match;
                if ((match = CityCommaState.Match(cityAndState)).Success || (match = CitySpaceState.Match(cityAndState)).Success)
                {
                    return IsCity(match.Groups[1].Value, new State(match.Groups[2].Value));
                }
                return false;
            }
            return false;
        }
    }
}
