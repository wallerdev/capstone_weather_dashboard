using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class CountyList
    {
        private static readonly List<County> Counties = new List<County>();

        private static readonly Dictionary<KeyValuePair<string, string>, County> CountyLookup =
            new Dictionary<KeyValuePair<string, string>, County>();

        static CountyList()
        {
            IEnumerable<string> entries = Properties.Resources.CountyGeocodes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                var county = new County(parts[0], new State(parts[1]), new Geocode(double.Parse(parts[2]), double.Parse(parts[3])));
                Counties.Add(county);
                CountyLookup[new KeyValuePair<string, string>(county.Name, county.State.Abbreviation)] = county;
            }
        }

        public static County FindClosestCounty(Geocode geocode)
        {
            Counties.Sort((a, b) => a.Geocode.DistanceTo(geocode).CompareTo(b.Geocode.DistanceTo(geocode)));
            return Counties.First();
        }

        public static IEnumerable<County> FindNearbyCounties(Geocode geocode, double rangeInMiles)
        {
            return Counties.Where(c => c.Geocode.DistanceTo(geocode) < rangeInMiles);
        }

        public static County GetCounty(string county, State state)
        {
            return CountyLookup[new KeyValuePair<string, string>(county, state.Abbreviation)];
        }
    }
}
