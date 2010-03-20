using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class AirportList
    {
        private static readonly List<Airport> Airports = new List<Airport>();
        static AirportList()
        {
            IEnumerable<string> entries = Properties.Resources.AirportGeocodes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').Select(part => part.Trim('"')).ToList();
                if(parts.Count < 5)
                {
                    
                }
                var airport = new Airport(parts[0], parts[1], new State(parts[2]), new Geocode(double.Parse(parts[3]), double.Parse(parts[4])));
                Airports.Add(airport);
            }
        }

        public Airport FindClosestAirport(Geocode geocode)
        {
            Airports.Sort((a, b) => a.Geocode.DistanceTo(geocode).CompareTo(b.Geocode.DistanceTo(geocode)));
            return Airports.First();
        }

        public IEnumerable<Airport> FindNearbyAirports(Geocode geocode, double rangeInMiles)
        {
            return Airports.Where(a => a.Geocode.DistanceTo(geocode) < rangeInMiles);
        }
    }
}
