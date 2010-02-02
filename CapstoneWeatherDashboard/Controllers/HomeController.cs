using System;
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
            var address = new Address("3410 Engineering Building", "East Lansing", "MI", "48824");
            var ncdc = new Ncdc();

            var incidents = ncdc.GetEvents(address, DateTime.Now.AddDays(-360).Date, DateTime.Now);
            return View(incidents);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
