using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStation
{
    public class ZipCode
    {
        public string Code { get; private set; }
        public Geocode Geocode { get; private set; }

        public ZipCode(string code, Geocode geocode)
        {
            Code = code;
            Geocode = geocode;
        }
    }
}
