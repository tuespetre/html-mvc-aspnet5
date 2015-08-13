using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.Framework.Internal;
using Microsoft.AspNet.Mvc.Rendering;
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
            var actionContext = context
                .HttpContext
                .RequestServices
                .GetService(typeof(IScopedInstance<ActionContext>)) as IScopedInstance<ActionContext>;

            var metadataProvider = context
                .HttpContext
                .RequestServices
                .GetService(typeof(IModelMetadataProvider)) as IModelMetadataProvider;

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
