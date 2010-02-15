using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherStation;

namespace CapstoneWeatherDashboard.Models
{
    public class Policy
    {
        public Address Address
        {
            get;
            private set;
        }

        public string FirstName
        {
            get;
            private set;
        }

        public string LastName
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }

        public string Number
        {
            get;
            private set;
        }

        public Policy(string firstName, string lastName, string number, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            Number = number;
            Address = address;
        }
    }
}
