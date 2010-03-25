namespace WeatherStation
{
    class UnknownAddress : Address
    {
        public override string FullAddress
        {
            get
            {
                return StreetAddress;
            }
        }

        public UnknownAddress(string address)
            : base(address, null, null, null, null, null, true)
        {
            
        }
    }
}
