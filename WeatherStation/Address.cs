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

        public string FullAddress
        {
            get
            {
                return FormatAddress(StreetAddress, City.Name, State.Abbreviation, ZipCode.Code);
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

        public static string FormatAddress(string streetAddress, string city, string state, string zipCode)
        {
            string address = "";
            if (!string.IsNullOrEmpty(streetAddress))
            {
                address += streetAddress + " ";
            }
            if (!string.IsNullOrEmpty(city))
            {
                address += city + " ";
            }
            if (!string.IsNullOrEmpty(state))
            {
                address += state + " ";
            }
            if (!string.IsNullOrEmpty(zipCode))
            {
                address += zipCode;
            }

            return address;
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county)
            : this(streetAddress, city, state, zipCode, county, null, false)
        {
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county, Geocode geocode)
            : this(streetAddress, city, state, zipCode, county, geocode, false)
        {
        }

        public Address(string streetAddress, string city, string state, string zipCode, string county, Geocode geocode, bool unknown)
        {
            StreetAddress = streetAddress;
            if(!unknown)
            {
                State = new State(state);
                City = CityList.GetCity(city, State);
                ZipCode = ZipCodeList.GetZipCode(zipCode);
                County = CountyList.GetCounty(county, State);
                Geocode = geocode;
            }
        }

        public static IEnumerable<Address> Search(string searchAddress)
        {
            return Search(searchAddress, false);
        }

        public static IEnumerable<Address> Search(string searchAddress, bool geocode)
        {
            var addressLookup = new AddressLookup();
            var addresses = new List<Address>();

            if (ZoneLookup.IsZone(searchAddress))
            {
                foreach (var zip in ZoneLookup.GetZipCodes(searchAddress))
                {
                    addresses.Add(addressLookup.GetAddressFromZipCode(zip));
                }
            }
            else if (ZipCodeList.IsZipCode(searchAddress))
            {
                ZipCode zipCode = ZipCodeList.GetZipCode(searchAddress);
                addresses.Add(addressLookup.GetAddressFromZipCode(zipCode));
            }
            else if (CityList.IsCityAndState(searchAddress))
            {
                var city = CityList.GetCity(searchAddress);
                addresses.Add(addressLookup.GetAddressFromCity(city));
            }
            else if (geocode)
            {
                var response = Geocoder.Search(searchAddress);
                addresses.Add(response);
            }
            else
            {
                addresses.Add(new UnknownAddress(searchAddress));
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
