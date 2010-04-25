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
            var date = HttpUtility.UrlDecode(Request.Form["d"]);
            var url = HttpUtility.UrlDecode(Request.Form["mi"]);
            var ev = HttpUtility.UrlDecode(Request.Form["et"]);
            var dates = new List<string>();
            var urls = new List<string>();
            var events = new List<string>();
            var restOfMapUrls = new List<string>();

            string restOfMapUrl = HttpUtility.UrlDecode(Request.Form["i"]);

            int size = 0;

            IEnumerable<string> markers = restOfMapUrl.Split(new[] { "&markers" }, StringSplitOptions.RemoveEmptyEntries);
            var searchedLocation = markers.First();
            markers = markers.Skip(1);
            while (markers.Count() > 0)
            {
                var markerGroup = markers.Take(32).ToList();
                markerGroup.Insert(0, searchedLocation);
                markers = markers.Skip(32);
                restOfMapUrls.Add(HttpUtility.UrlDecode(string.Join("", markerGroup.Select(m => "&markers" + m).ToArray())));
                events.Add(ev);
                urls.Add(url);
                dates.Add(date);
                size++;
            }

            ViewData["size"] = size;
            ViewData["restOfMapUrls"] = restOfMapUrls.ToArray();
            ViewData["events"] = events.ToArray();
            ViewData["urls"] = urls.ToArray();
            ViewData["dates"] = dates.ToArray();

            return ViewPdf(null);
        }

        public ActionResult AllIncidentsAsPdf()
        {
            var dates = Request.Form.GetValues("d[]").ToList();
            var urls = Request.Form.GetValues("mi[]").ToList();
            var events = Request.Form.GetValues("et[]").ToList();
            var restOfMapUrls = Request.Form.GetValues("i[]").ToList();
            int size = Request.Form.GetValues("d[]").Length;


            for (int i = 0; i < size; i++)
            {
                var date = HttpUtility.UrlDecode(dates[i]);
                var url = HttpUtility.UrlDecode(urls[i]);
                var ev = HttpUtility.UrlDecode(events[i]);
                var restOfMapUrl = HttpUtility.UrlDecode(restOfMapUrls[i]);

                dates.RemoveAt(i);
                urls.RemoveAt(i);
                events.RemoveAt(i);
                restOfMapUrls.RemoveAt(i);

                IEnumerable<string> markers = restOfMapUrl.Split(new[] { "&markers" }, StringSplitOptions.RemoveEmptyEntries);
                var searchedLocation = markers.First();
                markers = markers.Skip(1);
                while (markers.Count() > 0)
                {
                    var markerGroup = markers.Take(32).ToList();
                    markerGroup.Insert(0, searchedLocation);
                    markers = markers.Skip(32);
                    restOfMapUrls.Insert(i, HttpUtility.UrlDecode(string.Join("", markerGroup.Select(m => "&markers" + m).ToArray())));
                    events.Insert(i, ev);
                    urls.Insert(i, url);
                    dates.Insert(i, date);
                    i++;
                    size++;
                }
                i--;
                size--;

            }


            ViewData["size"] = size - 1;
            ViewData["restOfMapUrls"] = restOfMapUrls.ToArray();
            ViewData["events"] = events.ToArray();
            ViewData["urls"] = urls.ToArray();
            ViewData["dates"] = dates.ToArray();

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

        public void AllIncidentsAsEmail()
        {
            string emails = HttpUtility.UrlDecode(Request.Form["e"]);
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

            byte[] pdf = GetPdf(null);

            var email = new EmailJobCreator(pdf, emails);
            email.Send();
        }
    }
}
