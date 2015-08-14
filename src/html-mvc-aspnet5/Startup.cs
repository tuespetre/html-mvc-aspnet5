using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using html_mvc_aspnet5.Models;
using html_mvc_aspnet5.Helpers;
using html_mvc_aspnet5.Services;
using System.Linq;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;

namespace html_mvc_aspnet5
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().ConfigureMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.OutputFormatters.Add(new TextHtmlOutputFormatter());
                config.OutputFormatters.Add(new MultipartJsonOutputFormatter());
            });

            services.AddSingleton<IItemRepository, InMemoryItemRepository>();
            services.AddScoped<HtmlMvcTagHelperContext>();
            services.AddScoped<MultiObjectResultContext>();
            services.AddScoped<LayoutViewModel>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
