using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapstoneWeatherDashboard.Models;

namespace CapstoneWeatherDashboard.Controllers
{
    public class PolicyController : Controller
    {
        IPolicyRepository _repository;

        public PolicyController()
            : this(new PolicyRepository())
        {
        }

        public PolicyController(IPolicyRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.ListAll());
        }

    }
}
