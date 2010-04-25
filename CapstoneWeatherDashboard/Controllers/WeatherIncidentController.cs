using System;
using System.Linq;
using System.Web.Mvc;
using InsurancePolicyRepository;
using WeatherStation;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherIncidentController : Controller
    {
        private static readonly GoogleGeocoder Geocoder = new GoogleGeocoder();
        private readonly IPolicyProvider _insurancePolicyProvider = new MockPolicyProvider();

        public ActionResult Index()
        {
            Address address = null;

            string searchDataHtml = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["addressSearch"]))
            {
                var addressSearched = Address.FormatAddress(Request.QueryString["address"], Request.QueryString["city"],
                                                            Request.QueryString["state"], Request.QueryString["zipCode"]);
                address = Address.Search(Request.QueryString["address"], Request.QueryString["city"],
                    Request.QueryString["state"], Request.QueryString["zipCode"], true).First();

                searchDataHtml += SearchDataEntry("Address Searched", addressSearched);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["geocodeSearch"]))
            {
                double latitude = double.Parse(Request.QueryString["latitude"]);
                double longitude = double.Parse(Request.QueryString["longitude"]);

                address = Geocoder.ReverseGeocode(latitude, longitude);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["policySearch"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["policyNumber"]))
                {
                    searchDataHtml += SearchDataEntry("Policy Number", Request.QueryString["policyNumber"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["policyHolderName"]))
                {
                    searchDataHtml += SearchDataEntry("Policy Holder Name", Request.QueryString["policyHolderName"]);
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

            if (!string.IsNullOrEmpty(Request.QueryString["incidentTypes"]))
            {
                searchDataHtml += SearchDataEntry("Incident Type", Request.QueryString["incidentTypes"]);
            }
            
            if(!string.IsNullOrEmpty(Request.QueryString["radius"]))
            {
                searchDataHtml += SearchDataEntry("Radius", Request.QueryString["radius"]);
            }

            searchDataHtml += SearchDataEntry("Address Found", address.FullAddress);

            searchDataHtml += SearchDataEntry("Latitude", address.Geocode.Latitude.ToString());
            searchDataHtml += SearchDataEntry("Longitude", address.Geocode.Longitude.ToString());


            DateTime startDate = DateTime.Parse(Request.QueryString["startDate"]);
            DateTime endDate = DateTime.Parse(Request.QueryString["endDate"]);


            searchDataHtml += SearchDataEntry("Start Date", startDate.ToShortDateString());
            searchDataHtml += SearchDataEntry("End Date", endDate.ToShortDateString());

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
            ViewData["searchDataHtml"] = searchDataHtml;
            return View();
        }

        private static string SearchDataEntry(string label, string value)
        {
            return "<tr><td><label>" + label + "</label></td><td>" + value + "</td></tr>";
        }
    }
}
