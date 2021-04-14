using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Platform
{
    public class SumEndpoint
    {
        public async Task Endpoint(HttpContext context, IDistributedCache cache)
        {
            var count = int.Parse((string) context.Request.RouteValues["count"] ?? string.Empty);
            var cacheKey = $"sum_{count}";
            var totalString = await cache.GetStringAsync(cacheKey);

            if (totalString == null)
            {
                long total = 0;
                for (var i = 1; i <= count; i++)
                {
                    total += 1;
                }
                totalString = $"({DateTime.Now.ToLongTimeString()} {total})";

                await cache.SetStringAsync(cacheKey, totalString,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                    });
            }

            await context.Response
                .WriteAsync($"({DateTime.Now.ToLongTimeString()}) total for {count}" +
                            $" values:\n{totalString}\n");
        }
    }
}