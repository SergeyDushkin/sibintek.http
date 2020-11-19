using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using sibintek.sibmobile.core;

namespace sibintek.http.middleware
{

    // TODO доделать или использовать 
    // https://github.com/twitchax/AspNetCore.Proxy
    public class ServiceRouteMiddleware
    {
        public ServiceRouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            var serviceName = context.Request.Headers["X_SERVICE_NAME"].FirstOrDefault();

            if (string.IsNullOrEmpty(serviceName))
            {
                await _next.Invoke(context);
                return;
            }

            string baseUri = "";

            // Return `true` to allow certificates that are untrusted/invalid
            //var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.ServerCertificateCustomValidationCallback = 
            //    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            HttpClient client = new HttpClient();

            var message = new HttpRequestMessage(new HttpMethod(context.Request.Method), baseUri);
            message.Content = new StreamContent(context.Request.Body);

            context.Request.Headers.Keys
                .ToList()
                .ForEach(r => message.Headers.Add(r, context.Request.Headers[r].ToList()));

            message.Headers.Add("X_IDENTITY_NAME", context.User.Identity.Name);
            
            var result = await client.SendAsync(message);
            
            //context.Response = result;
        }
    }

    public static class ServiceRouteMiddlewareExtensions
    {
        public static IApplicationBuilder UseServiceRoute(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServiceRouteMiddleware>();
        }
    }
}
