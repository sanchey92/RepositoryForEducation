using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApplication.Models;

namespace WebApplication
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _nextDelegate;

        public TestMiddleware(RequestDelegate next)
        {
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext context, DataContext dataContext)
        {
            if (context.Request.Path == "/test")
            {
                await context.Response
                    .WriteAsync(
                        $"There are {dataContext.Products.Count()} products\n" +
                        $"There are {dataContext.Categories.Count()} categories\n" +
                        $"There are {dataContext.Suppliers.Count()} suppliers\n");
            }
            else
            {
                await _nextDelegate(context);
            }
        }
    }
}