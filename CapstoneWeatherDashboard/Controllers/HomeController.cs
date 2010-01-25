using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneWeatherDashboard.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "It rained yesterday.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
