using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WeatherStation.WeatherEventProviders
{
    public class WeatherUnderground
    {
        private const string WeatherHistoryCsvUrl =
            "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?format=1";

        private const string WeatherHistoryUrl =
            "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html";

        private static readonly WebClient WebClient = new WebClient();

        #region IWeatherEventProvider Members

        /// <summary>
        /// This function doesn't technically use wunderground's api. Instead it uses a normal web request to a csv file.
        /// A limitation of this approach is that we would have to read through each day and decide if a significant event happened
        /// on that day from the data given.
        /// Right now this function just returns all data for a given day.
        /// </summary>
        /// <returns>Weather Incidents for the location</returns>
        public IEnumerable<WeatherIncident> GetEvents(string airportCode, DateTime date)
        {
            string requestUrl = string.Format(WeatherHistoryCsvUrl, airportCode, date.Year, date.Month,
                                              date.Day);
            string response = WebClient.DownloadString(requestUrl);

            return ParseWundergroundResponse(airportCode, response, date);
        }

        #endregion

        private static IEnumerable<WeatherIncident> ParseWundergroundResponse(string airportCode, string responseString,
                                                                              DateTime date)
        {
            var moreInfoUrl = new Uri(string.Format(WeatherHistoryUrl, airportCode, date.Year, date.Month, date.Day));
            Airport currentAirpot = AirportList.GetAirport(airportCode);
            var locationOfAirport = Address.Search(string.Empty, currentAirpot.City, currentAirpot.State.Abbreviation, string.Empty, false);

            // Remove HTML comments and line breaks
            responseString =
                Regex.Replace(responseString, "<!--.*?-->", string.Empty, RegexOptions.Singleline).Replace("<br />", "");

            IEnumerable<string> rows = responseString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            rows = rows.Skip(1); // Skip header row


            var incidents = new List<WeatherIncident>();
            foreach (string row in rows)
            {
                var entry = new WeatherUndergroundEntry(date, row);
                WeatherIncidentType incidentType = WeatherIncidentClassifier.Classify(entry);
                if (incidentType != WeatherIncidentType.Unclassified)
                {
                    incidents.Add(new WeatherIncident(locationOfAirport, incidentType,
                                                      date, moreInfoUrl));
                }
            }
            return incidents;
        }
    }
}