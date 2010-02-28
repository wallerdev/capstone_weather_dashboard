using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace WeatherStation
{
    public class State
    {
        readonly string _name;
        readonly TextInfo _info = new CultureInfo("en-US", false).TextInfo;

        public string Name
        {
            get { return _info.ToTitleCase(_name); }
        }

        public string Abbreviation
        {
            get { return StateAbbreviations[_name].ToUpperInvariant(); }
        }

        static readonly Dictionary<string, string> StateAbbreviations = new Dictionary<string, string>
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
            {"armed forces americas", "aa"},
            {"armed forces canada", "ae"},
            {"armed forces pacific", "ap"}
        };

        public State(string state)
        {
            if (StateAbbreviations.Keys.Contains(state.ToLower()))
            {
                _name = state.ToLower();
            }
            else if (StateAbbreviations.Values.Contains(state.ToLower()))
            {
                _name = StateAbbreviations.First(v => v.Value == state.ToLowerInvariant()).Key;
            }
            else
            {
                throw new ArgumentOutOfRangeException(state, "Unknown State");
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (State)) return false;
            return Equals((State) obj);
        }

        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._name, _name);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}
