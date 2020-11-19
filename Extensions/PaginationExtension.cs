using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using sibintek.sibmobile.core;

namespace sibintek.http.context
{
    public static class PaginationExtension
    {
        public static IEnumerable<T> UsePagination<T>(this IQueryable<T> data, HttpContext context)
        {
            var pageFromRequest = context.Request.Headers["page"].FirstOrDefault();
            var limitFromRequest = context.Request.Headers["limit"].FirstOrDefault();

            int page = 1;
            int limit = 100;

            if (!string.IsNullOrEmpty(pageFromRequest))
            {
                int.TryParse(pageFromRequest, out page);
            }

            if (!string.IsNullOrEmpty(limitFromRequest))
            {
                int.TryParse(limitFromRequest, out limit);
            }

            var pagination = data.AsPagination(page, limit);

            context.Response.Headers.Add("total", pagination.TotalResults.ToString());
            context.Response.Headers.Add("pages", pagination.TotalPages.ToString());
            context.Response.Headers.Add("limit", pagination.ResultsPerPage.ToString());
            context.Response.Headers.Add("page", pagination.CurrentPage.ToString());

            return pagination.Items;
        }
    }
}
