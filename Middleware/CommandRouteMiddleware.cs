using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using sibintek.sibmobile.core;

namespace sibintek.http.middleware
{
    public class CommandRouteMiddleware
    {
        public CommandRouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context, ICommandHandlerFactory factory)
        {
            var commandName = context.Request.Headers["X_COMMAND_TYPE"].FirstOrDefault();
            var correlationId = context.Request.Headers["X_CORRELATION_ID"].FirstOrDefault();

            if (string.IsNullOrEmpty(commandName))
            {
                await _next.Invoke(context);
                return;
            }
            
            var types = factory.GetHandlers(commandName);
            foreach (var type in types)
            {
                var service = context.RequestServices.GetService(type);
                var commandType = type.GetInterfaces()
                    .FirstOrDefault()
                    .GetGenericArguments()
                    .FirstOrDefault();
                
                var resolver = new DefaultJsonHttpContextResolver();
                var command = await resolver.Resolve(context, commandType);

                var method = service.GetType().GetMethod("Handle");
                var task = (Task)method.Invoke(service, new [] { command });
                await task.ConfigureAwait(false);
            }
        }
    }

    public static class CommandRouteMiddlewareExtensions
    {
        public static IApplicationBuilder UseCommandRoute(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CommandRouteMiddleware>();
        }
    }
}
