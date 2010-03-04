using System;
using System.Collections.Generic;
using System.Linq;
using WeatherStation.WeatherEventProviders;

namespace WeatherStation
{
    static class WeatherIncidentClassifier
    {
        private static readonly IEnumerable<string> HailList = new[] { "hail" };
        private static readonly IEnumerable<string> WindList = new[] { "highwind", "tstm wind", "thunderstorm winds", "thunderstorm wind", "high wind", "thunderstorm wind g50", "strong wind" };
        private static readonly IEnumerable<string> TornadoList = new[] { "tornado", "tornado winds", "funnel cloud" };
        private static readonly IEnumerable<string> TropicalStormList = new[] { "tropical storm" };
        private static readonly IEnumerable<string> HurricaneList = new string[] {"hurricane"};
        private static readonly IEnumerable<string> FloodList = new[] { "flood", "flood/flash flood", "urban flooding", "flash flood" };
        private static readonly IEnumerable<string> WildfireList = new[] { "wild fire" };
        private static readonly IEnumerable<string> LightningList = new[] { "lightning" };
        private static readonly IEnumerable<string> WinterStormList = new[] { "lake effect snow", "heavy lake snow", "heavy snow/freezing rain", "winter storm",
            "heavy snow", "snow", "blizzard", "lake-effect snow", "winter weather"};
        private static readonly IEnumerable<string> ColdList = new[] { "cold wave", "extreme cold/wind chill", "cold/wind chill" };
        private static readonly IEnumerable<string> IceStormList = new[] { "ice storm", "freezing rain" };


        /// <summary>
        /// Classifies a string as a weather incident type. Throws an argument exception 
        /// if it cannot be classified.
        /// </summary>
        /// <param name="value">The string to classify</param>
        /// <returns>The corresponding Weather Incident Type</returns>
        public static WeatherIncidentType Classify(string value)
        {
            var listLookup = new Dictionary<IEnumerable<string>, WeatherIncidentType>
            {
                {FloodList, WeatherIncidentType.Flood},
                {WindList, WeatherIncidentType.Wind},
                {HailList, WeatherIncidentType.Hail},
                {TornadoList, WeatherIncidentType.Tornado},
                {WinterStormList, WeatherIncidentType.WinterStorm},
                {ColdList, WeatherIncidentType.ExtremeCold},
                {IceStormList, WeatherIncidentType.IceStorm},
                {TropicalStormList, WeatherIncidentType.TropicalStorm},
                {LightningList, WeatherIncidentType.Lightning},
                {HurricaneList, WeatherIncidentType.Hurricane},
                {WildfireList, WeatherIncidentType.Wildfire}
            };

            foreach (var lookup in listLookup)
            {
                if (lookup.Key.Contains(value.ToLowerInvariant()))
                {
                    return lookup.Value;
                }
            }

            return WeatherIncidentType.Unclassified;
        }

        /// <summary>
        /// Classifies an entry of weather underground data as a weather event.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>The corresponding Weather Incident Type</returns>
        public static WeatherIncidentType Classify(WeatherUndergroundEntry entry)
        {
            // TODO: Document these rules
            // TODO: Generalize these rules to be used with other web services
            if (entry.Precipitation > 0.5)
            {
                if (entry.Temperature >= 32.0)
                {
                    return WeatherIncidentType.Flood;
                }
                return WeatherIncidentType.WinterStorm;
            }
            if (entry.WindSpeed > 25)
            {
                return WeatherIncidentType.Wind;
            }
            
            throw new ArgumentException("Unknown WeatherIncidentType");
        }
    }
}
