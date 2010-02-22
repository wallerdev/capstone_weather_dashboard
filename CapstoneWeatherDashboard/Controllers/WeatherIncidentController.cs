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
                string cityFromQueryString = Request.QueryString["city"];
                string stateFromQueryString = Request.QueryString["state"];
                string zipFromQueryString = Request.QueryString["zipCode"];
                if( string.IsNullOrEmpty(cityFromQueryString) || string.IsNullOrEmpty(stateFromQueryString))
                {
                    address = new Address(zipFromQueryString);
                }
                else
                {
                    address = new Address(Request.QueryString["address"], cityFromQueryString,
                    stateFromQueryString, Request.QueryString["zip"]);    
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["geocodeSearch"]))
            {
                double latitude = double.Parse( Request.QueryString["latitude"]);
                double longitude = double.Parse( Request.QueryString["longitude"]);

                GoogleGeocodeResponse response = GoogleGeocoder.ReverseGeocode(latitude, longitude);

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

            ViewData["airportCode"] = closestAirportCode;
            ViewData["state"] = state;
            ViewData["county"] = county;
            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;

            return View();
        }
    }
}
