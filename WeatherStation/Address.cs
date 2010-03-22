using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using System;

namespace WeatherStation
{
    public class Address
    {
        private static readonly GoogleGeocoder Geocoder = new GoogleGeocoder();
        private static readonly ZoneLookup ZoneLookup = new ZoneLookup();
        private static readonly AddressLookup AddressLookup = new AddressLookup();
        private static readonly CityList CityList = new CityList();
        private static readonly ZipCodeList ZipCodeList = new ZipCodeList();
        private static readonly CountyList CountyList = new CountyList();

        public string FullAddress
        {
            get
            {
                string address = "";
                if (!string.IsNullOrEmpty(StreetAddress))
                {
                    address += StreetAddress + " ";
                }
                if (City != null)
                {
                    address += City.Name + " ";
                }
                if (State != null)
                {
                    address += State.Abbreviation + " ";
                }
                if (ZipCode != null)
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

        public City City
        {
            get;
            set;
        }

        public State State
        {
            get;
            set;
        }

        public ZipCode ZipCode
        {
            get;
            set;
        }

        public County County
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
            if (!string.IsNullOrEmpty(state))
            {
                State = new State(state);
            }
            if(CityList.IsCity(city, State))
            {
                City = CityList.GetCity(city, State);
            }
            else
            {
                Trace.WriteLine(string.Format("{0}, {1}", city, State.Name));
            }
            
            ZipCode = ZipCodeList.GetZipCode(zipCode);
            County = CountyList.GetCounty(county, State);
            Geocode = geocode;
        }

        public static IEnumerable<Address> Search(string searchAddress)
        {
            return Search(searchAddress, false);
        }

        public static IEnumerable<Address> Search(string searchAddress, bool geocode)
        {
            var addresses = new List<Address>();

            if (ZoneLookup.IsZone(searchAddress))
            {
                foreach (var zip in ZoneLookup.GetZipCodes(searchAddress))
                {
                    addresses.Add(AddressLookup.GetAddressFromZipCode(zip));
                }
            }
            else if (ZipCodeList.IsZipCode(searchAddress))
            {
                ZipCode zipCode = ZipCodeList.GetZipCode(searchAddress);
                addresses.Add(AddressLookup.GetAddressFromZipCode(zipCode));
            }
            else if (CityList.IsCityAndState(searchAddress))
            {
                var city = CityList.GetCity(searchAddress);
                addresses.Add(AddressLookup.GetAddressFromCity(city));
            }
            else if (geocode)
            {
                var response = Geocoder.Search(searchAddress);
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
            var response = Geocoder.Geocode(this);
            if (!string.IsNullOrEmpty(response.StreetAddress))
            {
                StreetAddress = response.StreetAddress;
            }
            if (response.City != null)
            {
                City = response.City;
            }
            if (response.County != null)
            {
                County = response.County;
            }
            if (response.State != null)
            {
                State = response.State;
            }
            if (response.ZipCode != null)
            {
                ZipCode = response.ZipCode;
            }
            Geocode = response.Geocode;
        }
    }
}
