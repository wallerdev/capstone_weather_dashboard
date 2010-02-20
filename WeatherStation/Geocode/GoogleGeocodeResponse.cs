namespace WeatherStation.Geocode
{
    public class GoogleGeocodeResponse
    {
        public string Address
        {
            get;
            set;
        }

        public string CountryNameCode
        {
            get;
            set;
        }

        public string CountryName
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string County
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string ZipCode
        {
            get;
            set;
        }

        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }
    }
}
