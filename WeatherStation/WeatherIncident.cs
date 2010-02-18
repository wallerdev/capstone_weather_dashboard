using System;

namespace WeatherStation
{
    /// <summary>
    /// This class has no logic, it just serves as a way to transfer
    /// weather incident data
    /// </summary>
    public class WeatherIncident
    {

        public string Location
        {
            get;
            set;
        }

        public WeatherIncidentType.Type EventType
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public Uri MoreInformationUrl
        {
            get;
            set;
        }

        public WeatherIncident(string location, WeatherIncidentType.Type eventType
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