using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
            
            services.AddControllers().AddNewtonsoftJson().AddXmlSerializerFormatters();

            services.Configure<MvcNewtonsoftJsonOptions>(options =>
            {
                options.SerializerSettings.NullValueHandling = 
                    Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            });
            
            services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddCors();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", 
                    new OpenApiInfo {Title = "WebApp", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, DataContext dataContext)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
            });
            
            SeedData.SeedDatabase(dataContext);
        }
    }
}