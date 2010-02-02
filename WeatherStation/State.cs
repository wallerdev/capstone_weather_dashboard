using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace WeatherStation
{
    public class State
    {
        string name;
        TextInfo info = new CultureInfo("en-US", false).TextInfo;

        public string Name
        {
            get { return info.ToTitleCase(name); }
        }

        public string Abbreviation
        {
            get { return stateAbbreviations[name].ToUpperInvariant(); }
        }

        static Dictionary<string, string> stateAbbreviations = new Dictionary<string, string>()
        {
            {"alabama", "al"},
            {"alaska", "ak"},
            {"american samoa", "as"},
            {"arizona", "az"},
            {"arkansas", "ar"},
            {"california", "ca"},
            {"colorado", "co"},
            {"connecticut", "ct"},
            {"delaware", "de"},
            {"district of columbia", "dc"},
            {"federated states of micronesia", "fm"},
            {"florida", "fl"},
            {"georgia", "ga"},
            {"guam", "gu"},
            {"hawaii", "hi"},
            {"idaho", "id"},
            {"illinois", "il"},
            {"indiana", "in"},
            {"iowa", "ia"},
            {"kansas", "ks"},
            {"kentucky", "ky"},
            {"louisiana", "la"},
            {"maine", "me"},
            {"marshall islands", "mh"},
            {"maryland", "md"},
            {"massachusetts", "ma"},
            {"michigan", "mi"},
            {"minnesota", "mn"},
            {"mississippi", "ms"},
            {"missouri", "mo"},
            {"montana", "mt"},
            {"nebraska", "ne"},
            {"nevada", "nv"},
            {"new hampshire", "nh"},
            {"new jersey", "nj"},
            {"new mexico", "nm"},
            {"new york", "ny"},
            {"north carolina", "nc"},
            {"north dakota", "nd"},
            {"northern mariana islands", "mp"},
            {"ohio", "oh"},
            {"oklahoma", "ok"},
            {"oregon", "or"},
            {"palau", "pw"},
            {"pennsylvania", "pa"},
            {"puerto rico", "pr"},
            {"rhode island", "ri"},
            {"south carolina", "sc"},
            {"south dakota", "sd"},
            {"tennessee", "tn"},
            {"texas", "tx"},
            {"utah", "ut"},
            {"vermont", "vt"},
            {"virgin islands", "vi"},
            {"virginia", "va"},
            {"washington", "wa"},
            {"west virginia", "wv"},
            {"wisconsin", "wi"},
            {"wyoming", "wy"},
            {"armed forces africa", "ae"},
            {"armed forces americas", "aa"},
            {"armed forces canada", "ae"},
            {"armed forces europe", "ae"},
            {"armed forces middle east", "ae"},
            {"armed forces pacific", "ap"}
        };

        public State(string state)
        {
            if (stateAbbreviations.Keys.Contains(state.ToLower()))
            {
                name = state.ToLower();
            }
            else if (stateAbbreviations.Values.Contains(state.ToLower()))
            {
                name = stateAbbreviations.First(v => v.Value == state.ToLowerInvariant()).Key;
            }
            else
            {
                throw new ArgumentOutOfRangeException(state, "Unknown State");
            }
        }
    }
}
