using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace sibintek.http.context
{
    public static partial class ODataExtension
    {
        private static string Get(this HttpRequest request, string name)
        {
            StringValues result;

            if (request.Query.TryGetValue(name, out result))
            {
                return result.FirstOrDefault();
            }

            return null;
        }
        private static T Get<T>(this HttpRequest request, string name)
        {
            StringValues result;

            if (request.Query.TryGetValue(name, out result))
            {
                var firstValue = result.FirstOrDefault();

                if (String.IsNullOrEmpty(firstValue))
                {
                    return default(T);
                }

                return (T)Convert.ChangeType(firstValue, typeof(T));
            }

            return default(T);
        }
    }
}
