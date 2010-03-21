using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace WeatherStation
{
    public class State
    {
        static readonly TextInfo _info = new CultureInfo("en-US", false).TextInfo;
        readonly string _name;
        

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
            {"arizona", "az"},
            {"arkansas", "ar"},
            {"california", "ca"},
            {"colorado", "co"},
            {"connecticut", "ct"},
            {"delaware", "de"},
            {"district of columbia", "dc"},
            {"florida", "fl"},
            {"georgia", "ga"},
            {"hawaii", "hi"},
            {"idaho", "id"},
            {"illinois", "il"},
            {"indiana", "in"},
            {"iowa", "ia"},
            {"kansas", "ks"},
            {"kentucky", "ky"},
            {"louisiana", "la"},
            {"maine", "me"},
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
            {"ohio", "oh"},
            {"oklahoma", "ok"},
            {"oregon", "or"},
            {"pennsylvania", "pa"},
            {"rhode island", "ri"},
            {"south carolina", "sc"},
            {"south dakota", "sd"},
            {"tennessee", "tn"},
            {"texas", "tx"},
            {"utah", "ut"},
            {"vermont", "vt"},
            {"virginia", "va"},
            {"washington", "wa"},
            {"west virginia", "wv"},
            {"wisconsin", "wi"},
            {"wyoming", "wy"},
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

        public static bool operator ==(State a, State b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if((object)a == null || ((object)b == null)) return false;
            return a._name == b._name;
        }

        public static  bool operator!=(State a, State b)
        {
            return !(a == b);
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
