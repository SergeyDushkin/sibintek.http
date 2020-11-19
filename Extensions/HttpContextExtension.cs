using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace sibintek.http.context
{
    public static class HttpContextExtension
    {
        public static async Task<T> DeserializeAsync<T>(this HttpContext context)
        {
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var json = await reader.ReadToEndAsync();
                return (T)JsonSerializer.Deserialize(json, typeof(T), options);
            }
        }
    }
}
