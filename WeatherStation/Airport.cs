using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class Airport
    {
        public string AirportCode { get; private set; }
        public string City { get; private set; }
        public State State { get; private set; }
        public Geocode Geocode{ get; private set; }

        public Airport(string airportCode, string city, State state, Geocode geocode)
        {
            AirportCode = airportCode;
            City = city;
            State = state;
            Geocode = geocode;
        }
    }
}
