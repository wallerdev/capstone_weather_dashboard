using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    /// <summary>
    /// This class has no logic, it just serves as a way to transfer
    /// weather incident data
    /// </summary>
    class WeatherIncident
    {

        public string Location { get; set; }

        public string EventType { get; set; }

        public WeatherIncident(string location, string eventType)
        {
            Location = location;
            EventType = eventType;
        }
    }
}