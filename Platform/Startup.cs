using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("countryName", typeof(CountryRouteConstrain));
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                Endpoint end = context.GetEndpoint();

                if (end != null)
                    await context.Response.WriteAsync($"{end.DisplayName} Selected \n");
                else
                    await context.Response.WriteAsync("No Endpoint Selected! \n");

                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("{first}/{second}/{*catchall}", async context =>
                // {
                //     await context.Response.WriteAsync("Request Was Routed\n");
                //     foreach (var kvp in context.Request.RouteValues)
                //     {
                //         await context.Response.WriteAsync($"{kvp.Key} : {kvp.Value}\n");
                //     }
                // });
                //
                // endpoints.MapGet("capital/{country:countryName}",  Capital.Endpoint);
                // endpoints.MapGet("size/{city?}", Population.Endpoint)
                //     .WithMetadata(new RouteNameMetadata("population"));

                endpoints.Map("{number:int}", async context =>
                {
                    await context.Response.WriteAsync($"Routed to the int endpoint");
                })
                    .WithDisplayName("Int Endpoint")
                    .Add(b => ((RouteEndpointBuilder)b).Order = 1);

                endpoints.Map("{number:double}", async context =>
                {
                    await context.Response.WriteAsync($"Routed to the double endpoint");
                })
                    .WithDisplayName("Double Endpoint")
                    .Add(b => ((RouteEndpointBuilder)b).Order = 2);
            });
            
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Terminal Middleware Reached");
            });
        }
    }
}