using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}
