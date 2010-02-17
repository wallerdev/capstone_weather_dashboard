using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading;
using System.Xml;

namespace WeatherStation.WeatherEventProviders
{
    public class WeatherUnderground : IWeatherEventProvider
    {
        private static string _airportCode;
        private List<WeatherIncident> _incidents = new List<WeatherIncident>();

        public IEnumerable<WeatherIncident> GetEvents(Address address, DateTime startDate, DateTime endDate)
        {
            _airportCode = GetClosestAirport(address.FullAddress);
            ParseWeatherEvents(startDate, endDate);
            return _incidents.AsEnumerable();
        }

        /// <summary>
        /// This function doesn't technically use wunderground's api. Instead it uses a normal web request to a csv file.
        /// A limitation of this approach is that we would have to read through each day and decide if a significant event happened
        /// on that day from the data given.
        /// Right now this function just returns all data for a given day.
        /// </summary>
        /// <returns></returns>
        private void ParseWeatherEvents(DateTime startDate, DateTime endDate)
        {
            const string weatherHistoryUrl
                            = "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?req_city=Bath&req_state=MI&req_statename=Michigan&format=1";

            // WaitHandles to wait on completion of requests.
            var handles = new List<WaitHandle>();

            DateTime currDate = startDate;
            while (currDate <= endDate)
            {
                string requestUrl = string.Format(weatherHistoryUrl, _airportCode, startDate.Year, startDate.Month,
                                                  startDate.Day);

                // Time how long it takes to get two pages
                handles.Add(GrabPageAsync(requestUrl, startDate));

                currDate.AddDays(1);
            }
            WaitHandle.WaitAll(handles.ToArray());
        }

        private EventWaitHandle GrabPageAsync(string url, DateTime startDate)
        {
            WebRequest request = WebRequest.Create(url);
            ManualResetEvent handle = new ManualResetEvent(false);
            var state = new WeatherUndergroundAsynchronousRequestUserState(startDate, handle, request);
            IAsyncResult result = request.BeginGetResponse(myClient_OpenReadCompleted, state);
            return handle;
        }

        private void myClient_OpenReadCompleted(IAsyncResult result)
        {
            try
            {
                var state = (WeatherUndergroundAsynchronousRequestUserState)result.AsyncState;
                WebResponse response = state.Request.EndGetResponse(result);

                var reader = new StreamReader(response.GetResponseStream());
                ParseWundergroundResponse(reader.ReadToEnd(), "KLAN", state.StartDate);
            }
            finally
            {
                var state = (WeatherUndergroundAsynchronousRequestUserState)result.AsyncState;
                state.EventHandle.Set();
            }
        }

        private void ParseWundergroundResponse(string responseString, string airportCode, DateTime date)
        {
            foreach (string row in responseString.Replace("<br />", "").Split('\n'))
            {
                try
                {
                    var entry = new WeatherUndergroundEntry(date, row);
                    if (entry.Precipitation > 0.5)
                    {
                        if (entry.Temperature >= 32.0)
                        {
                            _incidents.Add(new WeatherIncident(airportCode, "Flood", date, date));
                        }
                        else
                        {
                            _incidents.Add(new WeatherIncident(airportCode, "Snow Storm", date, date));
                        }
                        break;
                    }
                    if (entry.WindSpeed > 25)
                    {
                        _incidents.Add(new WeatherIncident(airportCode, "HighWind", date, date));
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

            var request = WebRequest.Create(apiUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                var reader = new StreamReader(response.GetResponseStream());
                var doc = new XmlDocument();
                doc.Load(reader);
                XmlNode node = doc.SelectSingleNode("/location/nearby_weather_stations/airport/station/icao");

                string closestAirportCode = node.FirstChild.Value;
                return closestAirportCode;
            }
        }
    }

    /// <summary>
    /// Internal class used for passing state information to the asynchronous request handler function
    /// </summary>
    internal class WeatherUndergroundAsynchronousRequestUserState
    {
        internal DateTime StartDate
        {
            get;
            set;
        }

        internal EventWaitHandle EventHandle
        {
            get;
            set;
        }

        internal WebRequest Request
        {
            get;
            set;
        }

        internal WeatherUndergroundAsynchronousRequestUserState(DateTime startDate, EventWaitHandle eventHandle, WebRequest request)
        {
            StartDate = startDate;
            EventHandle = eventHandle;
            Request = request;
        }
    }
}