using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using sibintek.sibmobile.core;

namespace sibintek.http
{
    public class DefaultJsonHttpContextResolver : IHttpContextResolver
    {
        public async Task<T> Resolve<T>(HttpContext context) where T : ICommand
        {
            return (T)(await Resolve(context, typeof(T)));
        }

        public async Task<object> Resolve(HttpContext context, Type type)
        {
            var command = (ICommand)(await Deserialize(context, type));
            command.Identity = ResolveIdentity(context);

            if (command is ILocalizeCommand)
            {
                ((ILocalizeCommand)command).Locale = ResolveLocale(context); 
            }

            return command;
        }

        private async Task<object> Deserialize(HttpContext context, Type type)
        {
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                //var options = new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true,
                //};
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject(json, type);
                
                //return JsonSerializer.Deserialize(json, type, options);
            }
        }

        private IIdentity ResolveIdentity(HttpContext context)
        {
            var name = context.Request.Headers["X_IDENTITY_NAME"].FirstOrDefault();
            return new Identity(name);
        }
        private string ResolveLocale(HttpContext context)
        {
            return context.Request.Headers["X_LOCALE_NAME"].FirstOrDefault();
        }
    }

}
