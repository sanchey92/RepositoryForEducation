using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;

namespace Platform
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITimeStamper, DefaultTimeStamper>();
            services.AddScoped<IResponseFormatter, TextResponseFormatter>();
            services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();
            services.AddScoped<IResponseFormatter, GuidService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/single", async context =>
                {
                    var formatter = context.RequestServices.GetService<IResponseFormatter>();
                    await formatter.Format(context, "Single Service");
                });

                endpoints.MapGet("/", async context =>
                {
                    var formatter = context.RequestServices
                        .GetServices<IResponseFormatter>()
                        .First(f => f.RichOutput);

                    await formatter.Format(context, "Multiple Service");
                });
            });
        }
    }
}