using System;
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
            IList<Policy> policies = _repository.ListAll();
            ViewData["Policies"] = new SelectList(policies, "Number", "Number");
            return View();
        }
    }
}
