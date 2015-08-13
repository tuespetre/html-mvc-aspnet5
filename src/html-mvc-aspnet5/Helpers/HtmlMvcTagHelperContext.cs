using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    /// <summary>
    /// This is here because trying to use the TagHelperContext's Items did
    /// not work between views.
    /// </summary>
    public class HtmlMvcTagHelperContext
    {
        public HtmlMvcTagHelperContext(IModelMetadataProvider provider)
        {
            MetadataProvider = provider;
        }

        public ViewDataDictionary CurrentData { get; set; }

        public IModelMetadataProvider MetadataProvider { get; private set; }

        public void Initialize(object model)
        {
            var modelDict = new ModelStateDictionary();
            CurrentData = new ViewDataDictionary(MetadataProvider, modelDict)
            {
                Model = model
            };
        }

        public object Value(string expression)
        {
            var explorer = ExpressionMetadataProvider.FromStringExpression(
                expression,
                CurrentData,
                MetadataProvider);

            return explorer.Model;
        }

        public void Scope(string expression)
        {
            var explorer = ExpressionMetadataProvider.FromStringExpression(
                expression, 
                CurrentData, 
                MetadataProvider);

            Initialize(explorer.Model ?? new object());
        }
    }
}
