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
            return ViewPdf(null);
        }
    }
}
