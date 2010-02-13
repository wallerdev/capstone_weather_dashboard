namespace WeatherStation
{
    public class Address
    {
        static ZipCodeLookup zipCodeLookup = new ZipCodeLookup();

        public string FullAddress
        {
            get
            {
                return string.Format("{0} {1}, {2} {3}", StreetAddress, City, State, ZipCode);
            }
        }

        public string StreetAddress
        {
            get;
            private set;
        }

        public string City
        {
            get;
            private set;
        }

        public State State
        {
            get;
            private set;
        }

        public string ZipCode
        {
            get;
            private set;
        }

        public Address(string streetAddress, string city, State state, string zipCode)
        {
            StreetAddress = streetAddress;
            City = city;
            State = state;
            ZipCode = zipCode;

        }

        public Address(string streetAddress, string city, string state, string zipCode)
            : this(streetAddress, city, new State(state), zipCode)
        {
        }

        public Address(string zipCode)
            : this(string.Empty, zipCodeLookup.GetCity(zipCode), zipCodeLookup.GetState(zipCode), zipCode)
        {
        }
    }
}
