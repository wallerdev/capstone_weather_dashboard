using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class City
    {
        public string Name { get; private set; }
        public State State { get; private set; }
        public Geocode Geocode { get; private set; }

        public City(string name, State state, Geocode geocode)
        {
            Name = name;
            State = state;
            Geocode = geocode;
        }
    }
}
