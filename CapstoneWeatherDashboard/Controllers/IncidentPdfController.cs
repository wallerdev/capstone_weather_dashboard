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
            string mapUrl = HttpUtility.UrlDecode(Request.QueryString["i"]);

            ViewData["date"] = date;
            ViewData["url"] = moreInfoUrl;
            ViewData["event"] = eventType;
            ViewData["mapurl"] = mapUrl;

            return ViewPdf(null);
        }
    }
}
