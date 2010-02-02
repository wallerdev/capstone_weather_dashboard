using System.Collections.Generic;
using System.IO;

namespace WeatherStation
{
    public static class ZipCodeManager
    {
        public static List<string> GetCountyAndStateFromZip(string zipCode)
        {
            //FOR NOW JUST RETURN HARD CODED COUNTY AND STATE
            List<string> returnData = new List<string>();
            returnData.Add("Ingham");
            returnData.Add("Mi");
            return returnData;

            // For now this just reads values from the csv
            // in the future we may want to move the table into a database or more likely
            // just hold the table in memory in some type of object
            using( var zipCodeCsv = new StreamReader("../../ZIP_CODES.csv"))
            {
                string row = string.Empty;
                while((row = zipCodeCsv.ReadLine()) != null )
                {
                    string[] items = row.Split(',');
                    if(items[0] == "\""+zipCode+"\"")
                    {
                        var returnValues = new List<string>();
                        returnValues.Add(items[5].Replace("\"",string.Empty));
                        returnValues.Add(items[4].Replace("\"", string.Empty));
                        return returnValues;
                    }
                }

                throw new KeyNotFoundException("Could not find zip code in file");
            }
        }
    }
}


