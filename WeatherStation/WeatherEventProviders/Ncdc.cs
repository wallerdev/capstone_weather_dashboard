﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace WeatherStation
{
    public class Ncdc : IWeatherEventProvider
    {
        private static IEnumerable<WeatherIncident> GetStormEvents(string stateTwoLetterCode, string countyInState)
        {
            // hardcoded url that we scrape the data from
            string baseNcdcUrl = "http://www4.ncdc.noaa.gov/cgi-win/wwcgi.dll?wwevent~storms";

            // the post parameters for making the call
            string parameters =
                string.Format("bdate=01%2F01%2F2006&edate=10%2F31%2F2009&STATE={0}&County={1}&ETYPE=*All&fscale=*All&hamt=&wspd=&injuries=&deaths=&pdamage=&cdamage=&Send=List+Storms", stateTwoLetterCode, countyInState);
            byte[] postData = Encoding.ASCII.GetBytes(parameters);

            // make a POST request to the url with the parameters given
            var request = WebRequest.Create(baseNcdcUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            // write the post data
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();

            // load the html response
            HtmlNode node;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                var reader = new StreamReader(response.GetResponseStream());

                var doc = new HtmlDocument();
                doc.LoadHtml(reader.ReadToEnd());

                // the description below is the best I could find in xpath that would describe the table of weather events
                // we may want to pull the string out into a config file
                node =
                    doc.DocumentNode.SelectSingleNode(
                        "//table[@summary='This Table displays the Events Query output by state and date.']");
            }

            //This goes through each row and pulls out the location and event type
            // This is fragile because we are depending on the html of the response
            var events = (from weatherEvent in node.SelectNodes("tr")
                          where weatherEvent.SelectSingleNode("td[@headers]") != null
                          select
                              new WeatherIncident(
                              weatherEvent.SelectSingleNode("td[@headers='h1']/a").InnerText.Trim(),
                              weatherEvent.SelectSingleNode("td[@headers='h4']").InnerText.Trim(),
                              DateTime.Parse(
                                  weatherEvent.SelectSingleNode("td[@headers='h2']").InnerText.Trim()),
                              DateTime.Parse(
                                  weatherEvent.SelectSingleNode("td[@headers='h2']").InnerText.Trim()))
                         );

            return events;
        }

        public IEnumerable<WeatherIncident> GetEvents(Address address, DateTime startDate, DateTime endDate)
        {
            ZipCodeLookup lookup = new ZipCodeLookup();
            return GetStormEvents(lookup.GetState(address.ZipCode.ToString()), lookup.GetCounty(address.ZipCode.ToString()));
        }
    }
}