using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    [TargetElement("*", Attributes = "bindattr-*")]
    [TargetElement("*", Attributes = "bindtext")]
    [TargetElement("*", Attributes = "bindhtml")]
    [TargetElement("*", Attributes = "bindeach")]
    public class AnyTagHelper : TagHelper
    {
        public AnyTagHelper(IHtmlHelper helper, IModelMetadataProvider metadataProvider, HtmlMvcTagHelperContext context)
        {
            //var services = contextAccessor.HttpContext.RequestServices;
            //this.metadataProvider = services.GetService(typeof(IModelMetadataProvider)) as IModelMetadataProvider;
            this.metadataProvider = metadataProvider;
            this.helper = helper;
            this.modelContext = context;
        }

        private IHtmlHelper helper;

        private IModelMetadataProvider metadataProvider;

        private HtmlMvcTagHelperContext modelContext;

        [HtmlAttributeName("bindtext")]
        public string BindText { get; set; }

        [HtmlAttributeName("bindhtml")]
        public string BindHtml { get; set; }

        [HtmlAttributeName("bindeach")]
        public string BindEach { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "bindattr-")]
        public IDictionary<string, string> BindAttributes { get; set; } = new Dictionary<string, string>();

        [ViewContext]
        public ViewContext Context { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var viewDictionary = modelContext.CurrentData;
            if (viewDictionary == null)
            {
                await base.ProcessAsync(context, output);
                return;
            }

            if (output.Attributes.ContainsName("bindattr-hidden"))
            {
                var bindhidden = output.Attributes["bindattr-hidden"].Value.ToString();
                var value = modelContext.Value(bindhidden);
                // 'Falsy values'
                if (value == null || string.Empty == (value as string) || value == (object)false)
                {
                    output.Attributes.Remove("hidden");
                }
                else
                {
                    output.Attributes.Add("hidden", value);
                }
            }

            foreach (var attribute in BindAttributes)
            {
                if (
                    attribute.Key == "hidden" ||
                    attribute.Key.StartsWith("bindattr-") ||
                    forbiddenForAny.Contains(attribute.Key) ||
                    (output.TagName.ToLower() == "view" && forbiddenForView.Contains(attribute.Key))
                )
                {
                    continue;
                }
                
                if (string.IsNullOrWhiteSpace(attribute.Value)) continue;
                var value = modelContext.Value(attribute.Value);
                output.Attributes.Add($"bindattr-{attribute.Key}", attribute.Value);
                output.Attributes.Add(attribute.Key, value);
            }

            if (!string.IsNullOrWhiteSpace(BindText))
            {
                var value = modelContext.Value(BindText);
                output.Attributes.Add("bindtext", BindText);
                output.Content.SetContent(helper.Encode(value));
            }
            else if (!string.IsNullOrWhiteSpace(BindHtml))
            {
                var value = modelContext.Value(BindHtml);
                output.Attributes.Add("bindhtml", BindHtml);
                output.Content.SetContent(helper.Raw(value).ToString());
            }
            else if (!string.IsNullOrWhiteSpace(BindEach))
            {
                var value = modelContext.Value(BindEach) as IEnumerable;
                output.Attributes.Add("bindeach", BindEach);
                if (value != null && value.Cast<object>().Any())
                {

                }
            }

            await base.ProcessAsync(context, output);
        }

        private static readonly string[] forbiddenForAny
            = new string[] { "bindtext", "bindhtml", "bindeach", "bindchildren", "bindnone" };

        private static readonly string[] forbiddenForView
            = new string[] { "name", "outer", "model", "scope" };
    }
}
