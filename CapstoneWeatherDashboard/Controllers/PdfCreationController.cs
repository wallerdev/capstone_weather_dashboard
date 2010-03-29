using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PDFBuilder;
using iTextSharp.text;


namespace CapstoneWeatherDashboard.Controllers
{
    /// <summary>
    /// Extends the MVC controller with functionlity for rendering PDF views.
    /// </summary>
    public class PdfCreationController : Controller
    {
        /// <summary>
        /// Renders an action result to a string. This is done by creating a fake http context
        /// and response objects and have that response send the data to a string builder
        /// instead of the browser.
        /// </summary>
        /// <param name="result">The action result to be rendered to string.</param>
        /// <returns>The data rendered by the given action result.</returns>
        protected string RenderActionResultToString(ActionResult result)
        {
            // Create memory writer.
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            // Create fake http context to render the view.
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request, fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                ControllerContext.RouteData,
                ControllerContext.Controller);
            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            // Render the view.
            result.ExecuteResult(fakeControllerContext);

            // Restore data.
            System.Web.HttpContext.Current = oldContext;

            // Flush memory and return output.
            memWriter.Flush();
            return sb.ToString();
        }

        /// <summary>
        /// Returns a PDF action result. This method renders the view to a string then
        /// use that string to generate a PDF file. The generated PDF file is then
        /// returned to the browser as binary content. The view associated with this
        /// action should render an XML compatible with iTextSharp xml format.
        /// </summary>
        /// <param name="model">The model to send to the view.</param>
        /// <returns>The resulted BinaryResult.</returns>
        protected ActionResult ViewPdf(object model)
        {
            HtmlToPdfBuilder builder = new HtmlToPdfBuilder(PageSize.LETTER);
            HtmlPdfPage page = builder.AddPage();
            string htmlText = RenderActionResultToString(View(model));
            page.AppendHtml(htmlText);
            byte[] file = builder.RenderPdf();
            
            // Send the binary data to the browser.
            return new BinaryResult(file, "application/pdf");
        }
    }
}
