using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Platform.Services;

namespace Platform
{
    public class WeatherEndpoint
    {
        private IResponseFormatter _formatter;

        public WeatherEndpoint(IResponseFormatter formatter)
        {
            _formatter = formatter;
        }
        
        public async Task Endpoint(HttpContext context)
        {
            await _formatter.Format(context, "Middleware Class: It is cloudy in Milan");
        }
    }
}