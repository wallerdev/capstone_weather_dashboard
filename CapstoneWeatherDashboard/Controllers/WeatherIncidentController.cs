using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeatherStation;

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
                Request.QueryString["state"], Request.QueryString["zip"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["geocodeSearch"]))
            {
                throw new NotImplementedException();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["policyNumberSearch"]))
            {
                throw new NotImplementedException();
            }

            if(address == null)
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
