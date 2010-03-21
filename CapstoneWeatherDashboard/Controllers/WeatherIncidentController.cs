using System;
using System.Web.Mvc;
using InsurancePolicyRepository;
using WeatherStation;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherIncidentController : Controller
    {
        private static readonly AirportList AirportList = new AirportList();

        private readonly IPolicyProvider _insurancePolicyProvider = new MockPolicyProvider();

        public ActionResult Index()
        {
            Address address = null;
            
            string searchStringAsEnglish = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["addressSearch"]))
            {
                searchStringAsEnglish += "Address: ";
                if (!string.IsNullOrEmpty( Request.QueryString["address"]))
                {
                    searchStringAsEnglish += Request.QueryString["address"] + " ";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["city"]))
                {
                    searchStringAsEnglish += Request.QueryString["city"] + ", ";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["state"]))
                {
                    searchStringAsEnglish += Request.QueryString["state"] + " ";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["zipCode"]))
                {
                    searchStringAsEnglish += Request.QueryString["zipCode"] + " ";
                }
                address = new Address(Request.QueryString["address"], Request.QueryString["city"],
                    Request.QueryString["state"], Request.QueryString["zipCode"]);
                address.GeocodeAddress();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["geocodeSearch"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["latitude"]))
                {
                    searchStringAsEnglish += "latitude: " + Request.QueryString["latitude"] + " ";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["longitude"]))
                {
                    searchStringAsEnglish += "longitude: " + Request.QueryString["longitude"] + " ";
                }

                double latitude = double.Parse(Request.QueryString["latitude"]);
                double longitude = double.Parse(Request.QueryString["longitude"]);

                address = new GoogleGeocoder().ReverseGeocode(latitude, longitude);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["policySearch"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["policyNumber"]))
                {
                    searchStringAsEnglish += "Policy Number: " + Request.QueryString["policyNumber"] + " ";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["policyHolderName"]))
                {
                    searchStringAsEnglish += "Policy Holder Name: " + Request.QueryString["policyHolderName"] + " ";
                }

                string policyNumber = Request.QueryString["policyNumber"];
                string policyHolderName = Request.QueryString["policyHolderName"];

                string policyStreetAddress = Request.QueryString["PolicyStreetAddress"];
                string policyCity = Request.QueryString["PolicyCity"];
                string policyState = Request.QueryString["PolicyState"];
                string policyZipCode = Request.QueryString["PolicyZipCode"];

                if (CanCreateAddressFromData(policyCity, policyState, policyZipCode))
                {
                    address = new Address(policyStreetAddress, policyCity, policyState, policyZipCode);
                }
                else
                {
                    PolicyInfo info =  _insurancePolicyProvider.GetPolicyThatMatchesNameOrNumber(policyNumber, policyHolderName);
                    address = info.PolicyHomeAddress;
                }

                address.GeocodeAddress();
            }

            if (address == null)
            {
                // TODO: change to some asp.net mvc validation system
                throw new ArgumentException("Invalid Search");
            }

            searchStringAsEnglish += "Start Date: " + Request.QueryString["startDate"] + " ";
            searchStringAsEnglish += "End Date: " + Request.QueryString["endDate"];
            if(!string.IsNullOrEmpty(Request.QueryString["incidentTypes"]))
            {
                searchStringAsEnglish += " Incident Type:" + Request.QueryString["incidentTypes"];
            }


            DateTime startDate = DateTime.Parse(Request.QueryString["startDate"]);
            DateTime endDate = DateTime.Parse(Request.QueryString["endDate"]);

            string closestAirportCode = AirportList.FindClosestAirport(address.Geocode).AirportCode;
            string state = address.State.Abbreviation;

            ViewData["homeAddress"] = address.FullAddress;
            ViewData["latitude"] = address.Geocode.Latitude;
            ViewData["longitude"] = address.Geocode.Longitude;
            ViewData["airportCode"] = closestAirportCode;
            ViewData["state"] = state;
            ViewData["county"] = address.County.Name;
            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            ViewData["incidentFilter"] = Request.QueryString["incidentTypes"];
            ViewData["searchStringAsEnglish"] = searchStringAsEnglish;
            return View();
        }

        private static bool CanCreateAddressFromData(string policyCity, string policyState, string policyZipCode)
        {
            return (!string.IsNullOrEmpty(policyCity) && !string.IsNullOrEmpty(policyState))
                        || !string.IsNullOrEmpty(policyZipCode);
        }
    }
}
