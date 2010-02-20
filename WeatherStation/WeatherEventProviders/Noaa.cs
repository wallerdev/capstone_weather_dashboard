using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using WeatherStation.NoaaService;

namespace WeatherStation.WeatherEventProviders
{
    public class Noaa
    {
        public void GetWeatherData(Geocode location)
        {
            // create a new proxy element from the NOAA WebReference
            var proxy = new ndfdXMLPortTypeClient();
            // get response data
            string data = proxy.NDFDgenByDay((decimal)location.Latitude, (decimal)location.Longitude, DateTime.Now.AddDays(-3).Date, "3",
                                             formatType.Item24hourly);

            StringReader reader = new StringReader(data);
            XDocument xmlDoc = XDocument.Load(reader);

            //using linq parse out the maximums and minimums
            var maximums = from tempvalue in xmlDoc.Descendants("temperature").Elements("value")
                           where tempvalue.Parent.Attribute("type").Value == "maximum"
                           select (string)tempvalue;

            var minimums = from tempvalue in xmlDoc.Descendants("temperature").Elements("value")
                           where tempvalue.Parent.Attribute("type").Value == "minimum"
                           select (string)tempvalue;



            // For now simply write out the lists to the console
            foreach (string max in maximums.ToList())
            {
                Console.WriteLine(max);
            }

            foreach (string min in minimums.ToList())
            {
                Console.WriteLine(min);
            }

            Console.WriteLine();
            Console.WriteLine(data);
        }
    }
}