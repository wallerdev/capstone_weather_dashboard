using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation.WeatherEventProviders
{
    class WeatherUndergroundEntry
    {
        public DateTime Date
        {
            get;
            set;
        }

        public double Temperature
        {
            get;
            set;
        }

        public double DewPoint
        {
            get;
            set;
        }

        public double Humidity
        {
            get;
            set;
        }

        public double SeaLevelPressure
        {
            get;
            set;
        }

        public double Visibility
        {
            get;
            set;
        }

        public string WindDirection
        {
            get;
            set;
        }

        public double WindSpeed
        {
            get;
            set;
        }

        public double GustWindSpeed
        {
            get;
            set;
        }

        public double Precipitation
        {
            get;
            set;
        }

        public string Events
        {
            get;
            set;
        }

        public string Conditions
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new weather underground entry. Throws a FormatException
        /// if it is unable to parse the csvRow.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="csvRow"></param>
        /// <exception cref="FormatException"></exception>
        public WeatherUndergroundEntry(DateTime date, string csvRow)
        {
            string[] csvElements = csvRow.Split(',');
            Date = date;
            Temperature = double.Parse(csvElements[1]);
            DewPoint = double.Parse(csvElements[2]);
            Humidity = double.Parse(csvElements[3]);
            SeaLevelPressure = double.Parse(csvElements[4]);
            Visibility = double.Parse(csvElements[5]);
            WindDirection = csvElements[6];
            WindSpeed = double.Parse(csvElements[7]);

            if (csvElements[8] == "-")
            {
                Precipitation = 0;
            }
            else
            {
                Precipitation = double.Parse(csvElements[8]);
            }
            if (csvElements[9] == "N/A")
            {
                Precipitation = 0;
            }
            else
            {
                Precipitation = double.Parse(csvElements[9]);
            }
            Events = csvElements[10];
            Conditions = csvElements[11];
        }
    }
}
