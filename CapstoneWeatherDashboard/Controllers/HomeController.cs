using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WeatherStation;

namespace CapstoneWeatherDashboard.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // NOTE RIGHT NOW THIS WILL ONLY WORK WITH NCDC FOR PLACES IN INGHAM COUNTY
            var address = new Address("3410 Engineering Building", "East Lansing", "MI", 48824);

            var events = new Ncdc();

            IEnumerable<WeatherIncident> response = events.GetEvents(address, DateTime.Now.AddDays(-360).Date, DateTime.Now);

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
