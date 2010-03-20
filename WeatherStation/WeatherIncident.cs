using System;
using System.Collections.Generic;

namespace WeatherStation
{
    /// <summary>
    /// This class represents a specific weather incident.
    /// </summary>
    public class WeatherIncident
    {
        public IEnumerable<Address> Locations
        {
            get;
            private set;
        }

        public WeatherIncidentType EventType
        {
            get;
            private set;
        }

        public string EventTypeString
        {
            get { return EventType.ToString(); }
        }

        public string EventTypeInWords
        {
            get
            {
                return new Dictionary<WeatherIncidentType, string>
                           {
                               {WeatherIncidentType.ExtremeCold, "Extreme Cold/Wind Chill"},
                               {WeatherIncidentType.Flood, "Flood"},
                               {WeatherIncidentType.Hail, "Hail"},
                               {WeatherIncidentType.Wind, "Wind"},
                               {WeatherIncidentType.IceStorm, "Freezing Rain/Ice Storm"},
                               {WeatherIncidentType.Tornado, "Tornado"},
                               {WeatherIncidentType.WinterStorm, "Winter Storm"},
                               {WeatherIncidentType.Lightning, "Lightning"},
                               {WeatherIncidentType.Hurricane, "Hurricane"},
                               {WeatherIncidentType.Wildfire, "Wild fire"},
                               {WeatherIncidentType.Unclassified, "Unclassified"}
                           }[EventType];
            }
        }

        public DateTime Date
        {
            get;
            private set;
        }

        public string DateString
        {
            get { return Date.ToString("yyyy-MM-dd");  }
        }

        public Uri MoreInformationUrl
        {
            get;
            private set;
        }

        public WeatherIncident(IEnumerable<Address> locations, WeatherIncidentType eventType
                                , DateTime date, Uri moreInformationUrl)
        {
            Locations = locations;
            EventType = eventType;
            Date = date;
            MoreInformationUrl = moreInformationUrl;
        }
    }
}