using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneWeatherDashboard.Models
{
    public class PolicyRepository : IPolicyRepository
    {
        public IList<Policy> ListAll()
        {
            return new Policy[] 
            {
                new Policy("Edward", "Waller", "903284023", new WeatherStation.Address("48864"))
            };
        }
    }
}
