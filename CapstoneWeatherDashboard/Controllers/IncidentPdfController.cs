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

        public ActionResult AllIncidentsAsPdf()
        {
            string[] dates = Request.Form.GetValues("d[]");
            string[] urls = Request.Form.GetValues("mi[]");
            string[] events = Request.Form.GetValues("et[]");
            string[] restOfMapUrls = Request.Form.GetValues("i[]");
            int size = Request.Form.GetValues("d[]").Length;
            ViewData["size"] = size;
            ViewData["restOfMapUrls"] = restOfMapUrls;
            ViewData["events"] = events;
            ViewData["urls"] = urls;
            ViewData["dates"] = dates;


            for (int i = 0; i < size; i++)
            {
                dates[i] = HttpUtility.UrlDecode(dates[i]);
                urls[i] = HttpUtility.UrlDecode(urls[i]);
                events[i] = HttpUtility.UrlDecode(events[i]);
                restOfMapUrls[i] = HttpUtility.UrlDecode(restOfMapUrls[i]);
            }

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
