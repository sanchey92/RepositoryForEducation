using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Platform.Models;

namespace Platform
{
    public class SumEndpoint
    {
        public async Task Endpoint(HttpContext context, CalculationContext dataContext)
        {
            var count = int.Parse((string) context.Request.RouteValues["count"] ?? string.Empty);
            long total = dataContext.Calculations
                .FirstOrDefault(c => c.Count == count)?.Result ?? 0;

            if (total == 0)
            {
                for (var i = 1; i <= count; i++)
                {
                    total += i;
                }

                dataContext.Calculations!
                    .Add(new Calculation() {Count = count, Result = total});
                await dataContext.SaveChangesAsync();
            }

            string totalString = $"({DateTime.Now.ToLongTimeString()}) {total}";
            await context.Response
                .WriteAsync($"({DateTime.Now.ToLongTimeString()}) Total for {count} " +
                            $" values:\n{totalString}\n");
        }
    }
}