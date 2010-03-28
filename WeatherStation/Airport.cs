using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class Airport : IEquatable<Airport>
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

        public bool Equals(Airport other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.AirportCode, AirportCode) && Equals(other.City, City) && Equals(other.State, State) && Equals(other.Geocode, Geocode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Airport)) return false;
            return Equals((Airport) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = AirportCode.GetHashCode();
                result = (result*397) ^ City.GetHashCode();
                result = (result*397) ^ State.GetHashCode();
                result = (result*397) ^ Geocode.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Airport left, Airport right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Airport left, Airport right)
        {
            return !Equals(left, right);
        }
    }
}
