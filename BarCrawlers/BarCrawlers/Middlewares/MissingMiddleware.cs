using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BarCrawlers.Middlewares
{
    public class MissingMiddleware
    {
        private readonly RequestDelegate next;
        public MissingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await next(httpContext);

            if (httpContext.Response.StatusCode == 404)
            {
                httpContext.Response.Redirect(string.Format("{0}://{1}/Home/Missing", httpContext.Request.Scheme, httpContext.Request.Host));
            }
        }
    }
}
