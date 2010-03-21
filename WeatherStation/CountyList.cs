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
        static CountyList()
        {
            IEnumerable<string> entries = Properties.Resources.CountyGeocodes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                var county = new County(parts[0], new State(parts[1]), new Geocode(double.Parse(parts[2]), double.Parse(parts[3])));
                Counties.Add(county);
            }
        }

        public County FindClosestCounty(Geocode geocode)
        {
            Counties.Sort((a, b) => a.Geocode.DistanceTo(geocode).CompareTo(b.Geocode.DistanceTo(geocode)));
            return Counties.First();
        }

        public IEnumerable<County> FindNearbyCounties(Geocode geocode, double rangeInMiles)
        {
            return Counties.Where(c => c.Geocode.DistanceTo(geocode) < rangeInMiles);
        }

        public static County GetCounty(string county, State state)
        {
            var temp = Counties.Where(c => c.Name == county && c.State == state).ToList();
            if(temp.Count != 1)
            {
                
            }
            return temp.Single(c => c.Name == county && c.State.Equals(state));
        }
    }
}
