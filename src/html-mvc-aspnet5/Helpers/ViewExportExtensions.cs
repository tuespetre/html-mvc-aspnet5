using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Routing.Template;
using Microsoft.Framework.DependencyInjection;

namespace html_mvc_aspnet5.Helpers
{
    public static class ViewExportExtensions
    {
        public static void UseViewExports(this IApplicationBuilder builder)
        {
            builder.Map("/exports", configuration =>
            {
                var engine = configuration.ApplicationServices.GetRequiredService<ICompositeViewEngine>();

                configuration.UseRouter(new TemplateRoute(
                    target: new ViewExportRouter(engine),
                    routeTemplate: "{controller}/{action}",
                    inlineConstraintResolver: null));
            });
        }
    }
}
