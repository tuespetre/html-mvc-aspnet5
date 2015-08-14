using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    [TargetElement("view", Attributes = "model")]
    [TargetElement("view", Attributes = "scope")]
    [TargetElement("view", Attributes = "name")]
    public class ViewTagHelper : TagHelper
    {
        public ViewTagHelper(IJsonHelper json, HtmlMvcTagHelperContext context)
        {
            jsonHelper = json;
            modelContext = context;
        }

        private const string MODEL_SCOPE = "model-scope";

        private IJsonHelper jsonHelper;

        private HtmlMvcTagHelperContext modelContext;

        public string Name { get; set; }

        public ModelExpression Model { get; set; }

        public string Scope { get; set; }

        public override int Order { get; } = 1;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(Model?.Name) && Model.Model != null)
            {
                modelContext.Initialize(Model.Model);

                var script = new TagBuilder("script");
                script.Attributes.Add("model", Model.Name);
                script.Attributes.Add("type", "application/json");
                script.InnerHtml = jsonHelper.Serialize(Model.Model).ToString();

                output.Attributes.Add("model", Model.Name);
                output.PreContent.SetContent(script.ToHtmlString(TagRenderMode.Normal).ToString());
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
