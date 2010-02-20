using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherStation.WeatherEventProviders;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherUndergroundWeatherIncidentController : Controller
    {
        public ActionResult Index()
        {
            string airportCode = Request.QueryString["airportCode"];
            DateTime date = DateTime.Parse(Request.QueryString["date"]);

            var weatherUnderground = new WeatherUnderground();
            return Json(weatherUnderground.GetEvents(airportCode, date));
        }

    }
}
