namespace WeatherStation
{
    public class Address
    {
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

        public string StateTwoLetterCode
        {
            get;
            private set;
        }

        public int ZipCode
        {
            get;
            private set;
        }

        public Address( string streetAddress, string city, string stateTwoLetterCode, int zipCode)
        {
            StreetAddress = streetAddress;
            City = city;
            StateTwoLetterCode = stateTwoLetterCode;
            ZipCode = zipCode;
        }
    }
}
