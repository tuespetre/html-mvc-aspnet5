using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Mvc.ModelBinding;

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

            var actionContext = services.GetService(typeof(IScopedInstance<ActionContext>)) 
                as IScopedInstance<ActionContext>;

            var metadataProvider = services.GetService(typeof(IModelMetadataProvider)) 
                as IModelMetadataProvider;

            var viewResult = new ViewResult
            {
                ViewData = new ViewDataDictionary(metadataProvider, new ModelStateDictionary())
                {
                    Model = context.Object
                }
            };

            return viewResult.ExecuteResultAsync(actionContext.Value);
        }
    }
}
