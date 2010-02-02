namespace WeatherStation
{
    public class Address
    {
        public string StreetAddress
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string StateTwoLetterCode
        {
            get;
            set;
        }

        public int ZipCode
        {
            get;
            set;
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
