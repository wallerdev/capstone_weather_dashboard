using System;
using System.Collections.Generic;

namespace WeatherStation
{
    public class WeatherIncidentType
    {
        readonly List<string> _floodList = new List<string>() { "flood", "flood/flash flood", "urban flooding", "flash flood" };
        readonly List<string> _windList = new List<string> { "highwind", "tstm wind", "thunderstorm winds" };
        readonly List<string> _hailList = new List<string> { "hail" };
        readonly List<string> _tornadoList = new List<string> { "tornado", "tornado winds", "funnel cloud" };
        readonly List<string> _winterStormList = new List<string> { "lake effect snow", "heavy lake snow", "heavy snow/freezing rain", "freezing rain", "winter storm",
                                                    "heavy snow", "snow"};
        readonly List<string> _coldList = new List<string> { "cold wave" };
        readonly List<string> _iceStormList = new List<string> { "ice storm" };

        public enum Type
        {
            Flood = 0,
            HighWind,
            WinterStorm,
            Hail,
            IceStorm,
            Tornado,
            ExtremeCold
        }

        /// <summary>
        /// Parses a string to a weather incident type. Throws an argument exception 
        /// if it cannot be parsed.
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>The WeatherIncidentType</returns>
        public Type Parse(string value)
        {
            if (_floodList.Contains(value.ToLower()))
            {
                return Type.Flood;
            }
            if (_windList.Contains(value.ToLower()))
            {
                return Type.HighWind;
            }
            if (_hailList.Contains(value.ToLower()))
            {
                return Type.Hail;
            }
            if (_tornadoList.Contains(value.ToLower()))
            {
                return Type.Tornado;
            }
            if (_winterStormList.Contains(value.ToLower()))
            {
                return Type.WinterStorm;
            }
            if (_coldList.Contains(value.ToLower()))
            {
                return Type.ExtremeCold;
            }
            if (_iceStormList.Contains(value.ToLower()))
            {
                return Type.IceStorm;
            }

            // could not parse
            throw new ArgumentException(string.Format("Could not parse '{0}' to a weather incident type", value));
        }
    }
}