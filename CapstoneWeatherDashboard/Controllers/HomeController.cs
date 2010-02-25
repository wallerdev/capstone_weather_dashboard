using System;
using System.Linq;
using System.Web.Mvc;
using WeatherStation;
using CapstoneWeatherDashboard.Models;
using System.Collections.Generic;

namespace CapstoneWeatherDashboard.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        IPolicyRepository _repository;

        public HomeController()
            : this(new PolicyRepository())
        {
        }

        public HomeController(IPolicyRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            List<Policy> policies = _repository.ListAll().ToList();
            
            policies.Sort((a, b) => a.Number.CompareTo(b.Number));
            ViewData["PolicyNumbers"] = new SelectList(policies, "Number", "Number");

            policies.Sort((a, b) => a.Name.CompareTo(b.Name));
            ViewData["PolicyNames"] = new SelectList(policies, "Number", "Name");

            var incidentTypes = Enum.GetNames(typeof (WeatherIncidentType));

            ViewData["IncidentTypes"] = new SelectList(incidentTypes);

            return View();
        }
    }
}
