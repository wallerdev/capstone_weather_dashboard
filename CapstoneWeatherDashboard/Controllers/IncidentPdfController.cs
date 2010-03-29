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
            string date = HttpUtility.UrlDecode(Request.QueryString["d"]);
            string moreInfoUrl = HttpUtility.UrlDecode(Request.QueryString["mi"]);
            string eventType = HttpUtility.UrlDecode(Request.QueryString["et"]);

            ViewData["date"] = date;
            ViewData["url"] = moreInfoUrl;
            ViewData["event"] = eventType;

            return ViewPdf(null);
        }
    }
}
