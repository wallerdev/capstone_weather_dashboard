using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapstoneWeatherDashboard.Models;

namespace CapstoneWeatherDashboard.Controllers
{
    public class IncidentPdfController : PdfCreationController
    {
        public ActionResult IncidentAsPdf()
        {
            string date = HttpUtility.UrlDecode(Request.Form["d"]);
            string moreInfoUrl = HttpUtility.UrlDecode(Request.Form["mi"]);
            string eventType = HttpUtility.UrlDecode(Request.Form["et"]);
            string restOfMapUrl = HttpUtility.UrlDecode(Request.Form["i"]);

            ViewData["date"] = date;
            ViewData["url"] = moreInfoUrl;
            ViewData["event"] = eventType;
            ViewData["restOfMapUrl"] = restOfMapUrl;

            return ViewPdf(null);
        }

        public void IncidentAsEmail()
        {
            string emails = HttpUtility.UrlDecode(Request.Form["e"]);
            string date = HttpUtility.UrlDecode(Request.Form["d"]);
            string moreInfoUrl = HttpUtility.UrlDecode(Request.Form["mi"]);
            string eventType = HttpUtility.UrlDecode(Request.Form["et"]);
            string restOfMapUrl = HttpUtility.UrlDecode(Request.Form["i"]);

            ViewData["date"] = date;
            ViewData["url"] = moreInfoUrl;
            ViewData["event"] = eventType;
            ViewData["restOfMapUrl"] = restOfMapUrl;

            byte[] pdf = GetPdf(null);

            var email = new EmailJobCreator(pdf, emails);
            email.Send();
        }
    }
}
