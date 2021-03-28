using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Platform
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                await next();
                await context.Response
                    .WriteAsync($"\nStatus code: {context.Response.StatusCode}");
            });
            
            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Method == HttpMethods.Get
                && ctx.Request.Query["custom"] == "true")
                {
                    await ctx.Response.WriteAsync("Custom Middleware \n");
                }

                await next();
            });

            app.UseMiddleware<QueryStringMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}