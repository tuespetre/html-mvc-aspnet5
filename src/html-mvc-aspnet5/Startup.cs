using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using html_mvc_aspnet5.Models;
using html_mvc_aspnet5.Helpers;
using html_mvc_aspnet5.Services;

namespace html_mvc_aspnet5
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddCaching();
            services.AddMvc().ConfigureMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.OutputFormatters.Add(new TextHtmlOutputFormatter());
                config.OutputFormatters.Add(new MultipartJsonOutputFormatter());
            });

            services.AddScoped<IItemRepository, InMemoryItemRepository>();
            services.AddScoped<HtmlMvcTagHelperContext>();
            services.AddScoped<MultiObjectResultContext>();
            services.AddScoped<LayoutViewModel>();

            /*services.Add(new ServiceDescriptor(
                typeof(TargetViewCollection),
                provider =>
                {
                    var targets = AppDomain
                        .CurrentDomain
                        .GetAssemblies()
                        .SelectMany(a => a.DefinedTypes)
                        .SelectMany(t => t.DeclaredMembers)
                        .SelectMany(m => m.CustomAttributes)
                        .Where(a => a.AttributeType == typeof(TargetViewAttribute));

                    return new TargetViewCollection
                    {
                        Views =
                        {

                        }
                    };
                },
                ServiceLifetime.Singleton));*/
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
    }
}
