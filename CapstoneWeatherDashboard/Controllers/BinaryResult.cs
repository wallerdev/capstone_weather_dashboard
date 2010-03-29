using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CapstoneWeatherDashboard.Controllers
{
    public class BinaryResult : ActionResult
    {
        private string ContentType;
        private byte[] ContentBytes;

        public BinaryResult(byte[] contentBytes, string contentType)
        {
            ContentBytes = contentBytes;
            ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = ContentType;

            var stream = new MemoryStream(ContentBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }

    }
}
