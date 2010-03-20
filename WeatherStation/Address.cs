using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using System;

namespace WeatherStation
{
    public class Address
    {
        private static readonly WebClient _webClient = new WebClient();
        private static readonly GoogleGeocoder _geocoder = new GoogleGeocoder();
        private static ZoneLookup _zoneLookup = new ZoneLookup();
        private static AddressLookup _addressLookup = new AddressLookup();

        public string FullAddress
        {
            get
            {
                string address = "";
                if (!string.IsNullOrEmpty(StreetAddress))
                {
                    address += StreetAddress + " ";
                }
                if (!string.IsNullOrEmpty(City))
                {
                    address += City + " ";
                }
                if (State != null)
                {
                    address += State.Abbreviation + " ";
                }
                if (!string.IsNullOrEmpty(ZipCode))
                {
                    address += ZipCode;
                }
                return address;
            }
        }

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

        public State State
        {
            get;
            set;
        }

        public string ZipCode
        {
            get;
            set;
        }

        public string County
        {
            get;
            set;
        }

        public Geocode Geocode
        {
            get;
            set;
        }

        public Address()
        {

        }

        public Address(string streetAddress, string city, string state, string zipCode)
            : this(streetAddress, city, state, zipCode, null, null)
        {
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county)
            : this(streetAddress, city, state, zipCode, county, null)
        {
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county, Geocode geocode)
        {
            StreetAddress = streetAddress;
            City = city;
            if (!string.IsNullOrEmpty(state))
            {
                State = new State(state);
            }
            ZipCode = zipCode;
            County = county;
            Geocode = geocode;
        }

        public static IEnumerable<Address> Search(string searchAddress)
        {
            return Search(searchAddress, false);
        }

        public static IEnumerable<Address> Search(string searchAddress, bool geocode)
        {
            var addresses = new List<Address>();

            if (_zoneLookup.IsZone(searchAddress))
            {
                foreach (var zip in _zoneLookup.GetZipCodes(searchAddress))
                {
                    addresses.Add(_addressLookup.GetAddressFromZipCode(zip));
                }
            }
            else if (_addressLookup.IsZipCode(searchAddress))
            {
                addresses.Add(_addressLookup.GetAddressFromZipCode(searchAddress));
            }
            else if (_addressLookup.IsCityAndState(searchAddress))
            {
                addresses.Add(_addressLookup.GetAddressFromCityAndState(searchAddress));
            }
            else if (geocode)
            {
                var response = _geocoder.Search(searchAddress);
                addresses.Add(response);
            }
            else
            {
                addresses.Add(new SearchAddress(searchAddress));
            }

            return addresses;
        }

        public void GeocodeAddress()
        {
            var response = _geocoder.Geocode(this);
            if (!string.IsNullOrEmpty(response.StreetAddress))
            {
                StreetAddress = response.StreetAddress;
            }
            if (!string.IsNullOrEmpty(response.City))
            {
                City = response.City;
            }
            if (!string.IsNullOrEmpty(response.County))
            {
                County = response.County;
            }
            if (response.State != null)
            {
                State = response.State;
            }
            if (!string.IsNullOrEmpty(response.ZipCode))
            {
                ZipCode = response.ZipCode;
            }
            Geocode = response.Geocode;
        }
    }
}
