using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherStation;

namespace CapstoneWeatherDashboard.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string zipcode = "48808";

            var events = new Ncdc("INGHAM", "MI");

            List<WeatherIncident> response = events.GetStormEvents();

            string output = string.Empty;
            foreach (var item in response)
            {
                output += item.Location;
                output += ": ";
                output += item.EventType;
                output += Environment.NewLine + "<br />";
            }

            ViewData["Message"] = output;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
