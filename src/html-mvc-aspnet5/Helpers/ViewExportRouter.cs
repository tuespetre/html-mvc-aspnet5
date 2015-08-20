using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    public class ViewExportRouter : IRouter
    {
        private ICompositeViewEngine viewEngine;

        public ViewExportRouter(ICompositeViewEngine viewEngine)
        {
            this.viewEngine = viewEngine;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            throw new NotImplementedException();
        }

        public async Task RouteAsync(RouteContext context)
        {
            var values = context.RouteData.Values;
            object controller;
            object action;

            if (
                values.TryGetValue("controller", out controller) &&
                values.TryGetValue("action", out action)
            )
            {
                var result = viewEngine.FindView(
                    context: new ActionContext(), 
                    viewName: action.ToString());

                if (result.Success)
                {
                    context.IsHandled = true;
                }
            }

            await Task.FromResult<object>(null);
        }
    }
}
