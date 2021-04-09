using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Platform
{
    public class Startup
    {
        public Startup(IConfiguration configService)
        {
            Configuration = configService;
        }

        private IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MessageOptions>(Configuration.GetSection("Location"));
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<LocationMiddleware>();

            app.Use(async (context, next) =>
            {
                var defaultDebug = Configuration["Logging:LogLevel:Default"];
                var environ = Configuration["ASPNETCORE_ENVIRONMENT"];
                var wsId = Configuration["WebService:Id"];
                var wsKey = Configuration["WebService:Key"];

                await context.Response
                    .WriteAsync($"The config setting is: {defaultDebug}\n" +
                                $"The env setting is: {environ}\n" +
                                $"The Secret Id is : {wsId}\n" +
                                $"The Secret Key is: {wsKey}\n");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    logger.LogDebug("Response for / started");
                    await context.Response.WriteAsync("Hello World!");
                    logger.LogDebug("Response for / completed");
                });
            });
        }
    }
}