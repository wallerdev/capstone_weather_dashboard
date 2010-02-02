using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    class ZipCodeLookupEntry
    {
        public string ZipCode
        {
            get;
            private set;
        }

        public string Latitude
        {
            get;
            private set;
        }

        public string Longitude
        {
            get;
            private set;
        }

        public string City
        {
            get;
            private set;
        }

        public string State
        {
            get;
            private set;
        }

        public string County
        {
            get;
            private set;
        }

        public ZipCodeLookupEntry(IList<string> data)
        {
            if (data.Count < 6)
            {
                throw new ArgumentException("Data must have at least 6 enttries", "data");
            }
            ZipCode = data[0];
            Latitude = data[1];
            Longitude = data[2];
            City = data[3];
            State = data[4];
            County = data[5];
        }
    }
}
