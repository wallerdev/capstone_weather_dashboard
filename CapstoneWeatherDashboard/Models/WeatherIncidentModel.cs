using System.Collections.Generic;
using WeatherStation;

namespace CapstoneWeatherDashboard.Models
{
    public class WeatherIncidentModel
    {
        public string HomeAddress
        {
            get;
            set;
        }

        public IEnumerable<WeatherIncident> Incidents
        {
            get;
            set;
        }

        public WeatherIncidentModel(string homeAddress, IEnumerable<WeatherIncident> incidents)
        {
            HomeAddress = homeAddress;
            Incidents = incidents;
        }
    }
}
