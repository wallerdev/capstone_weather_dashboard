﻿using System;
using System.Collections.Generic;
using System.Linq;
using WeatherStation.WeatherEventProviders;

namespace WeatherStation
{
    static class WeatherIncidentClassifier
    {
        private static readonly IEnumerable<string> FloodList = new[] { "flood", "flood/flash flood", "urban flooding", "flash flood" };
        private static readonly IEnumerable<string> WindList = new[] { "highwind", "tstm wind", "thunderstorm winds", "thunderstorm wind", "high wind", "thunderstorm wind g50" };
        private static readonly IEnumerable<string> HailList = new[] { "hail" };
        private static readonly IEnumerable<string> TornadoList = new[] { "tornado", "tornado winds", "funnel cloud" };
        private static readonly IEnumerable<string> WinterStormList = new[] { "lake effect snow", "heavy lake snow", "heavy snow/freezing rain", "freezing rain", "winter storm",
            "heavy snow", "snow", "blizzard"};
        private static readonly IEnumerable<string> ColdList = new[] { "cold wave" };
        private static readonly IEnumerable<string> IceStormList = new[] { "ice storm" };
        private static readonly IEnumerable<string> FogList = new[] { "dense fog", "fog" };


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
                {WindList, WeatherIncidentType.HighWind},
                {HailList, WeatherIncidentType.Hail},
                {TornadoList, WeatherIncidentType.Tornado},
                {WinterStormList, WeatherIncidentType.WinterStorm},
                {ColdList, WeatherIncidentType.ExtremeCold},
                {IceStormList, WeatherIncidentType.IceStorm},
                {FogList, WeatherIncidentType.DenseFog}
            };

            foreach (var lookup in listLookup)
            {
                if (lookup.Key.Contains(value.ToLowerInvariant()))
                {
                    return lookup.Value;
                }
            }

            throw new ArgumentException(string.Format("Could not parse '{0}' to a weather incident type", value));
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
                return WeatherIncidentType.HighWind;
            }
            
            throw new ArgumentException("Unknown WeatherIncidentType");
        }
    }
}