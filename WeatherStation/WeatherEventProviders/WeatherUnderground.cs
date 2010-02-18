using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WeatherStation.WeatherEventProviders
{
    public class WeatherUnderground : IWeatherEventProvider
    {
        private const string _weatherHistoryCsvUrl =
            "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html?format=1";

        private const string _weatherHistoryUrl =
            "http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/DailyHistory.html";

        private readonly List<WeatherIncident> _incidents = new List<WeatherIncident>();
        private readonly object _incidentsLock = new object();
        private readonly ManualResetEvent doneEvent = new ManualResetEvent(false);
        private readonly WebClient _webClient = new WebClient();
        private int _pagesLoading;
        private string _airportCode;


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
            _airportCode = GetClosestAirport(address.FullAddress);

            for (DateTime current = startDate; current <= endDate; current = current.AddDays(1))
            {
                string requestUrl = string.Format(_weatherHistoryCsvUrl, _airportCode, startDate.Year, startDate.Month,
                                                  startDate.Day);
                GrabPageAsync(requestUrl, startDate);
                Interlocked.Increment(ref _pagesLoading);
            }

            // Wait for all requests to finish
            doneEvent.WaitOne();

            return _incidents;
        }

        #endregion

        private void GrabPageAsync(string url, DateTime startDate)
        {
            WebRequest request = WebRequest.Create(url);
            var state = new WeatherUndergroundAsynchronousRequestUserState(startDate, request);
            request.BeginGetResponse(myClient_OpenReadCompleted, state);
        }

        private void myClient_OpenReadCompleted(IAsyncResult result)
        {
            var state = (WeatherUndergroundAsynchronousRequestUserState)result.AsyncState;
            try
            {
                WebResponse response = state.Request.EndGetResponse(result);

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    // TODO: Change this to the correct airport code 
                    ParseWundergroundResponse(reader.ReadToEnd(), state.StartDate);
                }
            }
            finally
            {
                if (Interlocked.Decrement(ref _pagesLoading) == 0)
                {
                    doneEvent.Set();
                }
            }
        }

        private void ParseWundergroundResponse(string responseString, DateTime date)
        {
            var moreInfoUrl = new Uri(string.Format(_weatherHistoryUrl, _airportCode, date.Year, date.Month, date.Day));

            // Remove HTML comments and line breaks
            responseString = Regex.Replace(responseString, "<!--.*?-->", string.Empty, RegexOptions.Singleline).Replace("<br />", "");

            IEnumerable<string> rows = responseString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            rows = rows.Skip(1); // Skip header row

            foreach (string row in rows)
            {
                var entry = new WeatherUndergroundEntry(date, row);
                lock (_incidentsLock)
                {
                    try
                    {
                        _incidents.Add(new WeatherIncident(_airportCode, WeatherIncidentClassifier.Classify(entry), date,
                                                           date, moreInfoUrl));
                    }
                    catch (ArgumentException)
                    {
                        // TODO: Handle this exception, should be able to classify all weather incidents
                    }
                }
            }
        }

        /// <summary>
        /// Wunderground historical data is based on airports. This function is provided to get the closest airport
        /// to a location using WunderGround's API.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private string GetClosestAirport(string location)
        {
            string apiUrl = string.Format("http://api.wunderground.com/auto/wui/geo/GeoLookupXML/index.xml?query={0}", location);
            string xml = _webClient.DownloadString(apiUrl);
            var document = XDocument.Parse(xml);
            var element = document.XPathSelectElement("/location/nearby_weather_stations/airport/station/icao");
            return element.Value;
        }

        #region Nested type: WeatherUndergroundAsynchronousRequestUserState

        /// <summary>
        /// Private class used for passing state information to the asynchronous request handler function
        /// </summary>
        private class WeatherUndergroundAsynchronousRequestUserState
        {
            internal WeatherUndergroundAsynchronousRequestUserState(DateTime startDate, WebRequest request)
            {
                StartDate = startDate;
                Request = request;
            }

            internal DateTime StartDate { get; private set; }

            internal WebRequest Request { get; private set; }
        }

        #endregion
    }
}