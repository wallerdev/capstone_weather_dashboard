using System;
using System.Web.Mvc;
using WeatherStation;
using WeatherStation.Geocode;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherIncidentController : Controller
    {
        public ActionResult Index()
        {
            Address address = null;

            if (!string.IsNullOrEmpty(Request.QueryString["addressSearch"]))
            {
                address = new Address(Request.QueryString["address"], Request.QueryString["city"],
                    Request.QueryString["state"], Request.QueryString["zipCode"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["geocodeSearch"]))
            {
                double latitude = double.Parse( Request.QueryString["latitude"]);
                double longitude = double.Parse( Request.QueryString["longitude"]);

                GoogleGeocodeResponse response = new GoogleGeocoder().ReverseGeocode(latitude, longitude);

                address = new Address(response.Address, response.City, response.State, response.ZipCode);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["policyNumberSearch"]))
            {
                throw new NotImplementedException();
            }

            if (address == null)
            {
                // TODO: change to some asp.net mvc validation system
                throw new ArgumentException("Invalid Search");
            }

            DateTime startDate = DateTime.Parse(Request.QueryString["startDate"]);
            DateTime endDate = DateTime.Parse(Request.QueryString["endDate"]);

            string closestAirportCode = address.FetchClosestAirportCode();
            string state = address.State.Abbreviation;
            string county = address.County;

            ViewData["homeAddress"] = address.FullAddress;
            ViewData["latitude"] = address.Latitude;
            ViewData["longitude"] = address.Longitude;
            ViewData["airportCode"] = closestAirportCode;
            ViewData["state"] = state;
            ViewData["county"] = county;
            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            ViewData["incidentFilter"] = Request.QueryString["incidentTypes"];

            return View();
        }
    }
}
