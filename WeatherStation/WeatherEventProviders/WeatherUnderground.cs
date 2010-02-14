using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace WeatherStation.WeatherEventProviders
{
    public class WeatherUnderground : IWeatherEventProvider
    {
        public IEnumerable<WeatherIncident> GetEvents(Address address, DateTime startDate, DateTime endDate)
        {
            string airportCode = GetClosestAirport(address.FullAddress);
            return GetWeatherEvents(airportCode, startDate, endDate);
        }

        /// <summary>
        /// This function doesn't technically use wunderground's api. Instead it uses a normal web request to a csv file.
        /// A limitation of this approach is that we would have to read through each day and decide if a significant event happened
        /// on that day from the data given.
        /// Right now this function just returns all data for a given day.
        /// </summary>
        /// <param name="airportCode"></param>
        /// <returns></returns>
        private static IEnumerable<WeatherIncident> GetWeatherEvents(string airportCode, DateTime startDate, DateTime endDate)
        {
            List<WeatherIncident> returnValues = new List<WeatherIncident>();
            string weatherHistoryUrl =
                "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?req_city=Bath&req_state=MI&req_statename=Michigan&format=1";

            while (startDate <= endDate)
            {
                string requestUrl = string.Format(weatherHistoryUrl, airportCode, startDate.Year, startDate.Month, startDate.Day);

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    List<WeatherIncident> incidents = ParseWundergroundResponse(reader.ReadToEnd(), airportCode, startDate);
                    returnValues.AddRange(incidents);
                }

                startDate = startDate.AddDays(1);
            }

            return returnValues.AsEnumerable();
        }

        private static List<WeatherIncident> ParseWundergroundResponse(string responseString, string airportCode, DateTime date)
        {
            var returnList = new List<WeatherIncident>();
            foreach (string row in responseString.Replace("<br />", "").Split('\n'))
            {
                try
                {
                    var entry = new WeatherUndergroundEntry(date, row);
                    if (entry.Precipitation > 0.5)
                    {
                        if (entry.Temperature >= 32.0)
                        {
                            returnList.Add(new WeatherIncident(airportCode, "Flood", date, date));
                        }
                        else
                        {
                            returnList.Add(new WeatherIncident(airportCode, "Snow Storm", date, date));
                        }
                        break;
                    }
                    if (entry.WindSpeed > 25)
                    {
                        returnList.Add(new WeatherIncident(airportCode, "HighWind", date, date));
                        break;
                    }
                }
                catch (FormatException)
                {
                    continue;
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
            return returnList;
        }

        /// <summary>
        /// Wunderground historical data is based on airports. THis function is provided to get the closest airport
        /// to a location using WunderGround's API.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static string GetClosestAirport(string location)
        {
            string apiUrl = "http://api.wunderground.com/auto/wui/geo/GeoLookupXML/index.xml?query=";

            apiUrl = string.Format("{0}{1}", apiUrl, location);

            HttpWebRequest request = WebRequest.Create(apiUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                XmlNode node = doc.SelectSingleNode("/location/nearby_weather_stations/airport/station/icao");

                string closestAirportCode = node.FirstChild.Value;
                return closestAirportCode;
            }
        }
    }
}
