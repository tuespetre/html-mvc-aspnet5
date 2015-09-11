using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.Framework.WebEncoders;
using System.IO;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    [TargetElement("view", Attributes = "model-name, model-data")]
    [TargetElement("view", Attributes = "model-persistent")]
    [TargetElement("view", Attributes = "scope")]
    [TargetElement("view", Attributes = "name")]
    public class ViewTagHelper : TagHelper
    {
        public ViewTagHelper(IJsonHelper json, HtmlMvcTagHelperContext context)
        {
            jsonHelper = json;
            modelContext = context;
        }

        private IJsonHelper jsonHelper;

        private HtmlMvcTagHelperContext modelContext;

        public string Name { get; set; }
        
        public string ModelName { get; set; }

        public string Scope { get; set; }
        
        public object ModelData { get; set; }

        public bool ModelPersistent { get; set; }

        public override int Order { get; } = 1;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(ModelName) && ModelData != null)
            {
                modelContext.Initialize(ModelData);

                var script = new TagBuilder("script");
                script.Attributes.Add("model", ModelName);
                script.Attributes.Add("type", "application/json");
                if (ModelPersistent)
                {
                    script.Attributes.Add("persistent", "");
                }
                script.InnerHtml = new StringHtmlContent(jsonHelper.Serialize(ModelData).ToString());

                output.Attributes.Add("model", ModelName);
                using (var writer = new StringWriter())
                {
                    script.WriteTo(writer, new HtmlEncoder());
                    output.PreContent.SetContent(writer.ToString());
                }
            }
            else if (!string.IsNullOrWhiteSpace(Scope))
            {
                output.Attributes.Add("scope", Scope);
                modelContext.Scope(Scope);
            }

            output.Attributes.Add("name", Name);

            await base.ProcessAsync(context, output);
        }
    }
}
