using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherStation
{
    public class ZoneLookup
    {
        private static readonly ZipCodeList ZipCodeList = new ZipCodeList();
        private static readonly Dictionary<string, IEnumerable<ZipCode>> ZoneToZipCodesLookup = new Dictionary<string, IEnumerable<ZipCode>>(); 


        static ZoneLookup()
        {
            IEnumerable<string> entries = Properties.Resources.ZoneZipCodes.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var parts = entry.Split(',').ToList();
                var zone = parts.First();
                var zips = parts.Skip(1).Select(z => ZipCodeList.GetZipCode(z));
                ZoneToZipCodesLookup[zone] = zips;
            }
        }

        public IEnumerable<ZipCode> GetZipCodes(string zones)
        {
            var zoneList = zones.Split(new[] { " - " }, StringSplitOptions.None);
            var prefix = zoneList[0].Substring(0, 3);
            zoneList[0] = zoneList[0].Remove(0, 3);

            var zips = new List<ZipCode>();
            foreach (var zone in zoneList)
            {
                if (zone.Contains(">"))
                {
                    var zoneRange = zone.Split('>');
                    var zoneBegin = int.Parse(zoneRange[0]);
                    var zoneEnd = int.Parse(zoneRange[1]);
                    for(int i = zoneBegin; i <= zoneEnd; i++)
                    {
                        zips.AddRange(ZoneToZipCodesLookup[prefix + i.ToString().PadLeft(3, '0')]);
                    }
                }
                else
                {
                    zips.AddRange(ZoneToZipCodesLookup[prefix + zone]);
                }
            }
            return zips;
        }

        public bool IsZone(string zone)
        {
            var zoneList = zone.Split(new[] { " - " }, StringSplitOptions.None);
            return ZoneToZipCodesLookup.ContainsKey(zoneList.FirstOrDefault());
        }
    }
}
