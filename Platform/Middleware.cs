using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Platform
{
    public class QueryStringMiddleware
    {
        private readonly RequestDelegate? _next;

        public QueryStringMiddleware() { }

        public QueryStringMiddleware(RequestDelegate nextDelegate)
        {
            _next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get
                && context.Request.Query["custom"] == "true")
            {
                await context.Response.WriteAsync("Class based Middleware \n");
            }

            if (_next != null)
                await _next(context);
        }
    }

    public class LocationMiddleware
    {
        private  RequestDelegate _next;
        private MessageOptions _options;

        public LocationMiddleware(RequestDelegate next, IOptions<MessageOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/location")
            {
                await context.Response
                    .WriteAsync($"{_options.CityName}, {_options.CountryName}");
            }
            else
            {
                await _next(context);
            }
        }
    }
}