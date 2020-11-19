using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using sibintek.sibmobile.core;

namespace sibintek.http.context
{
    public static partial class ODataExtension
    {
        public static IQueryable<T> UseODataPagination<T>(this IQueryable<T> data, HttpContext context)
        {
            var skip = context.Request.Get<int>("$skip");
            var limit = context.Request.Get<int>("$limit");

            skip = skip.Normalize(0);
            limit = limit.Normalize(1, 1000);

            var total = data.LongCount();
            var result = data.Skip(skip).Take(limit);

            context.Response.Headers.Add("total", total.ToString());

            return result;
        }
    }
}
