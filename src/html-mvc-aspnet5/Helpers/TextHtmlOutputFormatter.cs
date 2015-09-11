using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Actions;
using Microsoft.AspNet.Mvc.ActionResults;
using Microsoft.AspNet.Mvc.Formatters;

namespace html_mvc_aspnet5.Helpers
{
    public class TextHtmlOutputFormatter : OutputFormatter
    {
        public TextHtmlOutputFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override bool CanWriteResult(OutputFormatterContext context, MediaTypeHeaderValue contentType)
        {
            return base.CanWriteResult(context, contentType)
                && !(context.Object is ViewResult);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterContext context)
        {
            var services = context.HttpContext.RequestServices;
            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            var actionContext = services.GetRequiredService<IActionContextAccessor>().ActionContext;
            var metadataProvider = services.GetRequiredService<IModelMetadataProvider>();
            var tempdataProvider = services.GetRequiredService<ITempDataProvider>();

            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(metadataProvider, actionContext.ModelState),
                TempData = new TempDataDictionary(httpContextAccessor, tempdataProvider)
            };

            return viewResult.ExecuteResultAsync(actionContext);
        }
    }
}
