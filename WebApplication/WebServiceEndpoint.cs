using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Models;

namespace WebApplication
{
    public static class WebServiceEndpoint
    {
        private const string BaseUrl = "api/products";

        public static void MapWebService(this IEndpointRouteBuilder app)
        {
            app.MapGet($"{BaseUrl}/{{id}}", async context =>
            {
                long key = long.Parse(context.Request.RouteValues["id"] as string ?? string.Empty);
                DataContext data = context.RequestServices.GetService<DataContext>();
                Product product = data.Products.Find(key);

                if (product == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    context.Response.ContentType = "application/json";
                    await context.Response
                        .WriteAsync(JsonSerializer.Serialize<Product>(product));
                }
            });

            app.MapGet(BaseUrl, async context =>
            {
                DataContext data = context.RequestServices.GetService<DataContext>();
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize<IEnumerable<Product>>(data?.Products));
            });

            app.MapPost(BaseUrl, async context =>
            {
                DataContext data = context.RequestServices.GetService<DataContext>();
                Product product = await JsonSerializer.DeserializeAsync<Product>(context.Request.Body);
                await data.AddAsync(product);
                await data.SaveChangesAsync();
                context.Response.StatusCode = StatusCodes.Status200OK;
            });
        }
    }
}