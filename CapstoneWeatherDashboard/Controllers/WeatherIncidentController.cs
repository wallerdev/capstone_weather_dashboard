using System;
using System.Linq;
using System.Web.Mvc;
using WeatherStation;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherIncidentController : Controller
    {
        public ActionResult Index()
        {
            string zipCode = Request.QueryString["zipCode"];
            if(string.IsNullOrEmpty(zipCode))
            {
                // default to 48824 for now if empty
                zipCode = "48824";
            }
            var address = new Address(zipCode);
            var ncdc = new Ncdc();

            var incidents = ncdc.GetEvents(address, DateTime.Now.AddDays(-360).Date, DateTime.Now);
            return View(incidents.OrderBy(incident => incident.StartDate).Reverse());

        }

    }
}
