using System;
using System.Linq;
using System.Web.Mvc;
using InsurancePolicyRepository;
using WeatherStation;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherIncidentController : Controller
    {
        private static readonly AirportList AirportList = new AirportList();
        private static readonly GoogleGeocoder Geocoder = new GoogleGeocoder();
        private readonly IPolicyProvider _insurancePolicyProvider = new MockPolicyProvider();

        public ActionResult Index()
        {
            Address address = null;

            string searchStringAsEnglish = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["addressSearch"]))
            {
                string formattedAddress = Address.FormatAddress(Request.QueryString["address"],
                                                                Request.QueryString["city"],
                                                                Request.QueryString["state"],
                                                                Request.QueryString["zipCode"]);
                address = Address.Search(Request.QueryString["address"], Request.QueryString["city"],
                    Request.QueryString["state"], Request.QueryString["zipCode"], true).First();

                searchStringAsEnglish = "Adddress: " + address.FullAddress + " ";
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

                address = Geocoder.ReverseGeocode(latitude, longitude);
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

                PolicyInfo info = _insurancePolicyProvider.GetPolicyThatMatchesNameOrNumber(policyNumber, policyHolderName);
                address = info.PolicyHomeAddress;

                address.GeocodeAddress();
            }

            if (address == null)
            {
                // TODO: change to some asp.net mvc validation system
                throw new ArgumentException("Invalid Search");
            }

            searchStringAsEnglish += "Start Date: " + Request.QueryString["startDate"] + " ";
            searchStringAsEnglish += "End Date: " + Request.QueryString["endDate"];
            if (!string.IsNullOrEmpty(Request.QueryString["incidentTypes"]))
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
    }
}
