using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace WeatherStation
{
    public class WeatherUnderground
    {
        public string GetHistoricalData(string location)
        {
            string airportCode = GetClosestAirport(location);

            return GetWeatherEvents(airportCode);
        }

        /// <summary>
        /// This function doesn't technically use wunderground's api. Instead it uses a normal web request to a csv file.
        /// A limitation of this approach is that we would have to read through each day and decide if a significant event happened
        /// on that day from the data given.
        /// Right now this function just returns all data for a given day.
        /// </summary>
        /// <param name="airportCode"></param>
        /// <returns></returns>
        private static string GetWeatherEvents(string airportCode)
        {
            string weatherHistoryUrl =
                "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?req_city=Bath&req_state=MI&req_statename=Michigan&format=1";
            weatherHistoryUrl = string.Format(weatherHistoryUrl, airportCode, 2010, 1, 25);

            HttpWebRequest request = WebRequest.Create(weatherHistoryUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                return reader.ReadToEnd();
            }
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
