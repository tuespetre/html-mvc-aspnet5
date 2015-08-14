using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using MultipartContent = System.Net.Http.MultipartContent;
using StringContent = System.Net.Http.StringContent;
using HttpContent = System.Net.Http.HttpContent;
using System.Text;
using Microsoft.Framework.Internal;

namespace html_mvc_aspnet5.Helpers
{
    public class MultipartJsonOutputFormatter : OutputFormatter
    {
        public MultipartJsonOutputFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/json"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override bool CanWriteResult(OutputFormatterContext context, MediaTypeHeaderValue contentType)
        {
            return base.CanWriteResult(context, contentType)
                && !(context.Object is ObjectResult);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterContext context)
        { 
            var reqServices = context.HttpContext.RequestServices;
            var multiContext = (MultiObjectResultContext)reqServices.GetService(typeof(MultiObjectResultContext));
            var jsonHelper = (IJsonHelper)reqServices.GetService(typeof(IJsonHelper));
            var content = new MultipartContent("json");
            var resultObject = context.Object;
            var resultObjectWasAdded = false;

            foreach (var entry in multiContext.AdditionalObjects)
            {
                object additional = resultObject;

                if (additional.GetType() != entry.Value)
                {
                    additional = reqServices.GetService(entry.Value);
                }
                else
                {
                    resultObjectWasAdded = true;
                }

                content.Add(ContentPart(jsonHelper, entry.Key, additional));
            }

            if (!resultObjectWasAdded)
            {
                content.Add(ContentPart(jsonHelper, resultObject.GetType().Name, resultObject));
            }

            var response = context.HttpContext.Response;
            response.ContentType = content.Headers.ContentType.ToString();
            await content.CopyToAsync(response.Body);
        }

        private HttpContent ContentPart(IJsonHelper helper, string name, object @object)
        {
            var json = helper.Serialize(@object);
            var part = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            part.Headers.ContentType.Parameters.Add(
                new System.Net.Http.Headers.NameValueHeaderValue("model", name));

            return part;
        }
    }
}
