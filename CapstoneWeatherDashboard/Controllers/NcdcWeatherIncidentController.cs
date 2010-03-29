using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherStation;
using WeatherStation.WeatherEventProviders;

namespace CapstoneWeatherDashboard.Controllers
{
    public class NcdcWeatherIncidentController : Controller
    {
        public ActionResult Index()
        {
            string filterAsString = Request.QueryString["incidentFilter"];
            WeatherIncidentType? filter;
            try
            {
                filter = (WeatherIncidentType)Enum.Parse(typeof(WeatherIncidentType), filterAsString);
            }
            catch (ArgumentException)
            {
                filter = null;
            }

            double radius;
            if (!double.TryParse(Request.QueryString["radius"], out radius))
            {
                radius = 15;
            }

            County mainCounty = CountyList.GetCounty(Request.QueryString["county"], new State(Request.QueryString["state"]));

            List<County> counties = new List<County> { mainCounty };
            counties.AddRange(CountyList.FindNearbyCounties(mainCounty.Geocode, radius));
            counties = counties.Distinct().ToList();

            var ncdc = new Ncdc();
            List<WeatherIncident> ncdcIncidents = new List<WeatherIncident>();
            foreach (var county in counties)
            {
                ncdcIncidents.AddRange(ncdc.GetEvents(
                    county.State,
                    county.Name,
                    DateTime.Parse(Request.QueryString["startDate"]),
                    DateTime.Parse(Request.QueryString["endDate"]), filter));
            }

            return Json(ncdcIncidents.Distinct(), JsonRequestBehavior.AllowGet);
        }
    }
}
