using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace sibintek.http.middleware
{
    public class HeaderAuthenticationMiddleware
    {
        public HeaderAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {                
                var userId = context.Request.Headers["X_IDENTITY_NAME"].FirstOrDefault();

                if (!string.IsNullOrEmpty(userId))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userId)
                    };

                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", 
                        ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    context.User = new ClaimsPrincipal(id);
                }
            }

            await _next.Invoke(context);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseHeaderAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderAuthenticationMiddleware>();
        }
    }
}
