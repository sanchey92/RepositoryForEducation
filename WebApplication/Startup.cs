using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Models;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        
        private IConfiguration Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionStrings:ProductConnection"]);
                options.EnableSensitiveDataLogging(true);
            });
            services.AddControllers();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, DataContext dataContext)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            // app.UseMiddleware<TestMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                
                // endpoints.MapWebService();
                endpoints.MapControllers();
            });
            SeedData.SeedDatabase(dataContext);
        }
    }
}