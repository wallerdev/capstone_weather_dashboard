using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WeatherStation.WeatherEventProviders
{
    public class WeatherUnderground : IWeatherEventProvider
    {
        private const string WeatherHistoryCsvUrl =
            "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?format=1";

        private const string WeatherHistoryUrl =
            "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html";

        private readonly WebClient _webClient = new WebClient();

        #region IWeatherEventProvider Members

        /// <summary>
        /// This function doesn't technically use wunderground's api. Instead it uses a normal web request to a csv file.
        /// A limitation of this approach is that we would have to read through each day and decide if a significant event happened
        /// on that day from the data given.
        /// Right now this function just returns all data for a given day.
        /// </summary>
        /// <returns>Weather Incidents for the location</returns>
        public IEnumerable<WeatherIncident> GetEvents(Address address, DateTime startDate, DateTime endDate)
        {
            string airportCode = GetClosestAirport(address.FullAddress);
            var incidents = new List<WeatherIncident>();

            for (DateTime current = startDate; current <= endDate; current = current.AddDays(1))
            {
                string requestUrl = string.Format(WeatherHistoryCsvUrl, airportCode, startDate.Year, startDate.Month,
                                                  startDate.Day);
                string response = _webClient.DownloadString(requestUrl);
                incidents.AddRange(ParseWundergroundResponse(airportCode, response, startDate));
            }

            return incidents;
        }

        #endregion

        private static IEnumerable<WeatherIncident> ParseWundergroundResponse(string airportCode, string responseString,
                                                                              DateTime date)
        {
            var moreInfoUrl = new Uri(string.Format(WeatherHistoryUrl, airportCode, date.Year, date.Month, date.Day));

            // Remove HTML comments and line breaks
            responseString =
                Regex.Replace(responseString, "<!--.*?-->", string.Empty, RegexOptions.Singleline).Replace("<br />", "");

            IEnumerable<string> rows = responseString.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            rows = rows.Skip(1); // Skip header row


            var incidents = new List<WeatherIncident>();
            foreach (string row in rows)
            {
                var entry = new WeatherUndergroundEntry(date, row);
                try
                {
                    incidents.Add(new WeatherIncident(airportCode, WeatherIncidentClassifier.Classify(entry), date,
                                                      date, moreInfoUrl));
                }
                catch (ArgumentException)
                {
                    // TODO: Handle this exception, should be able to classify all weather incidents
                }
            }
            return incidents;
        }

        /// <summary>
        /// Wunderground historical data is based on airports. This function is provided to get the closest airport
        /// to a location using WunderGround's API.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private string GetClosestAirport(string location)
        {
            string apiUrl = string.Format("http://api.wunderground.com/auto/wui/geo/GeoLookupXML/index.xml?query={0}",
                                          location);
            string xml = _webClient.DownloadString(apiUrl);
            var document = XDocument.Parse(xml);
            var element = document.XPathSelectElement("/location/nearby_weather_stations/airport/station/icao");
            return element.Value;
        }
    }
}