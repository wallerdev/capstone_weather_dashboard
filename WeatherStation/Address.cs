namespace WeatherStation
{
    public class Address
    {
        static ZipCodeLookup zipCodeLookup = new ZipCodeLookup();

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

        public int ZipCode
        {
            get;
            private set;
        }

        public Address(string streetAddress, string city, string state, int zipCode)
        {
            StreetAddress = streetAddress;
            City = city;
            State = new State(state);
            ZipCode = zipCode;
        }

        public Address(string zipCode)
        {
            StreetAddress = string.Empty;
            City = zipCodeLookup.GetState(zipCode);
        }
    }
}
