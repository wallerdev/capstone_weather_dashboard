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
            // default to 48824 for now if empty
            string zipCode = GetQueryStringOrDefault("zipCode","48824");
            string startDateAsString = GetQueryStringOrDefault("startDate",DateTime.Now.AddDays(-360).Date.ToShortDateString());
            string endDateAsString = GetQueryStringOrDefault("endDate",DateTime.Now.ToShortDateString());

            DateTime startDate = DateTime.Parse(startDateAsString);
            DateTime endDate = DateTime.Parse(endDateAsString);

            var address = new Address(zipCode);
            var ncdc = new Ncdc();

            var incidents = ncdc.GetEvents(address, startDate, endDate);
            return View(incidents.OrderBy(incident => incident.StartDate).Reverse());

        }

        private string GetQueryStringOrDefault(string name, string defaultValue)
        {
            string value = Request.QueryString[name];
            if(string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            return value;
        }
    }
}
