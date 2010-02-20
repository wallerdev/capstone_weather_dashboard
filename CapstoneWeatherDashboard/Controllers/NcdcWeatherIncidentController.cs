﻿using System;
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
            var ncdc = new Ncdc();
            var ncdcIncidents = ncdc.GetEvents(
                new State(Request.QueryString["state"]),
                Request.QueryString["county"],
                DateTime.Parse(Request.QueryString["startDate"]),
                DateTime.Parse(Request.QueryString["endDate"]));
            return Json(ncdcIncidents, JsonRequestBehavior.AllowGet);
        }
    }
}