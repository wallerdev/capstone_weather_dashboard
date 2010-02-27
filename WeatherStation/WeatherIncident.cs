using System;
using System.Collections.Generic;

namespace WeatherStation
{
    /// <summary>
    /// This class represents a specific weather incident.
    /// </summary>
    public class WeatherIncident
    {
        public string Location
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
                               {WeatherIncidentType.DenseFog, "Dense Fog"},
                               {WeatherIncidentType.ExtremeCold, "Extreme Cold"},
                               {WeatherIncidentType.Flood, "Flood"},
                               {WeatherIncidentType.Hail, "Hail"},
                               {WeatherIncidentType.HighWind, "High Wind"},
                               {WeatherIncidentType.IceStorm, "Ice Storm"},
                               {WeatherIncidentType.Tornado, "Tornado"},
                               {WeatherIncidentType.WinterStorm, "Winter Storm"}
                           }[EventType];
            }
        }

        public DateTime StartDate
        {
            get;
            private set;
        }

        public string StartDateString
        {
            get { return StartDate.ToString("yyyy-MM-dd");  }
        }

        public DateTime EndDate
        {
            get;
            private set;
        }

        public Uri MoreInformationUrl
        {
            get;
            private set;
        }

        public WeatherIncident(string location, WeatherIncidentType eventType
                                , DateTime startDate, DateTime endDate, Uri moreInformationUrl)
        {
            Location = location;
            EventType = eventType;
            StartDate = startDate;
            EndDate = endDate;
            MoreInformationUrl = moreInformationUrl;
        }
    }
}