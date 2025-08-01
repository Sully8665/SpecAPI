using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SpecAPI.Middleware
{
    public class RequestValidation
    {
        private readonly RequestDelegate _next;

        public RequestValidation(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Basic validation logic
            if (!context.Request.Headers.ContainsKey("X-API-KEY"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key missing");
                return;
            }

            await _next(context);
        }
    }
}
