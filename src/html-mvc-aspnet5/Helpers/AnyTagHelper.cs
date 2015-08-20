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
using System.Reflection;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    [TargetElement("*", Attributes = "bindattr-*")]
    [TargetElement("*", Attributes = "bindtext")]
    [TargetElement("*", Attributes = "bindhtml")]
    [TargetElement("*", Attributes = "bindcount")]
    [TargetElement("*", Attributes = "bindsome")]
    [TargetElement("*", Attributes = "bindnone")]
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

        [HtmlAttributeName("bindcount")]
        public string BindCount { get; set; }

        [HtmlAttributeName("bindtext")]
        public string BindText { get; set; }

        [HtmlAttributeName("bindhtml")]
        public string BindHtml { get; set; }

        [HtmlAttributeName("bindsome")]
        public string BindSome { get; set; }

        [HtmlAttributeName("bindnone")]
        public string BindNone { get; set; }

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

            foreach (var attribute in BindAttributes)
            {
                if (
                    attribute.Key.StartsWith("bindattr-") ||
                    forbiddenForAny.Contains(attribute.Key) ||
                    (output.TagName.ToLower() == "view" && forbiddenForView.Contains(attribute.Key))
                )
                {
                    continue;
                }

                var value = modelContext.Value(attribute.Value);
                output.Attributes.Add($"bindattr-{attribute.Key}", attribute.Value);

                if (true.Equals(value))
                {
                    output.Attributes.Add(attribute.Key, string.Empty);
                }
                else if (!false.Equals(value))
                {
                    output.Attributes.Add(attribute.Key, value);
                }
            }

            if (!string.IsNullOrWhiteSpace(BindCount))
            {
                output.Attributes.Add("bindcount", BindCount);

                var value = modelContext.Value(BindCount) as IEnumerable;

                if (value == null)
                {
                    output.Content.SetContent(0.ToString());
                }
                else
                {
                    output.Content.SetContent(value.Cast<object>().Count().ToString());
                }
            }
            else if (!string.IsNullOrWhiteSpace(BindText))
            {
                output.Attributes.Add("bindtext", BindText);

                var value = modelContext.Value(BindText);
                output.Content.SetContent(helper.Encode(value));
            }
            else if (!string.IsNullOrWhiteSpace(BindHtml))
            {
                output.Attributes.Add("bindhtml", BindHtml);

                var value = modelContext.Value(BindHtml);
                output.Content.SetContent(helper.Raw(value).ToString());
            }
            else if (!string.IsNullOrWhiteSpace(BindSome))
            {
                output.Attributes.Add("bindsome", BindSome);
                var value = modelContext.Value(BindSome) as IEnumerable;

                if (value != null && value.Cast<object>().Any())
                {
                    ViewDataDictionary originalData;
                    TagHelperContent template;
                    TagHelperContent buffer;

                    originalData = modelContext.CurrentData;
                    modelContext.Initialize(new object());
                    buffer = new DefaultTagHelperContent();
                    template = await context.GetChildContentAsync();

                    // commit a8fd85d to aspnet/Razor will make this cleaner:
                    //   buffer.Append(await context.GetChildContentAsync(useCachedResult: false));
                    // for now just use reflection to hack it up
                    var delegateField = context.GetType().GetRuntimeFields().Single(f => f.Name == "_getChildContentAsync");
                    var @delegate = delegateField.GetValue(context);
                    var targetField = delegateField.FieldType.GetRuntimeProperties().Single(p => p.Name == "Target");
                    var @target = targetField.GetValue(@delegate);
                    var hackField = @target.GetType().GetRuntimeFields().Single(f => f.Name == "_childContent");

                    foreach (var item in value)
                    {
                        hackField.SetValue(@target, null);
                        modelContext.Initialize(item);
                        buffer.Append(await context.GetChildContentAsync());
                    }

                    output.Content.SetContent(buffer);
                    modelContext.CurrentData = originalData;
                }
                else
                {
                    output.Attributes.Add("hidden", true);
                }
            }
            else if (!string.IsNullOrWhiteSpace(BindNone))
            {
                output.Attributes.Add("bindnone", BindNone);
                var value = modelContext.Value(BindNone) as IEnumerable;

                if (value == null || value.Cast<object>().Any())
                {
                    output.Attributes.Add("hidden", true);
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
