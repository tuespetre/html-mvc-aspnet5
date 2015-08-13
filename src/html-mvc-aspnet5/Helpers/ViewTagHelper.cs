using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    [TargetElement("view", Attributes = "model")]
    [TargetElement("view", Attributes = "scope")]
    public class ViewTagHelper : TagHelper
    {
        public ViewTagHelper(IHtmlHelper html, IJsonHelper json, HtmlMvcTagHelperContext context)
        {
            htmlHelper = html;
            jsonHelper = json;
            modelContext = context;
        }

        private const string MODEL_SCOPE = "model-scope";

        private IHtmlHelper htmlHelper;

        private IJsonHelper jsonHelper;

        private HtmlMvcTagHelperContext modelContext;

        public ModelExpression Model { get; set; }

        public ModelExpression Scope { get; set; }

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
            else if (!string.IsNullOrWhiteSpace(Scope?.Name))
            {
                output.Attributes.Add("scope", Scope.Name);
                modelContext.Scope(Scope.Name);
            }

            await base.ProcessAsync(context, output);
        }
    }
}
