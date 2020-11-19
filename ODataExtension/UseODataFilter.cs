using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace sibintek.http.context
{
    public static partial class ODataExtension
    {
        // TODO
        // Нужно найти готовое решение
        public static IQueryable<T> UseODataFilter<T>(this IQueryable<T> data, HttpContext context)
        {
            var filter = context.Request.Get<string>("$filter");

            if (String.IsNullOrEmpty(filter))
            {
                return data;
            }

            var sections = filter.Split(new string[] { " and ", " or " }, StringSplitOptions.RemoveEmptyEntries);
            var firstSection = sections.FirstOrDefault();

            var fieldIdx = firstSection.IndexOf(" ");
            var operatorIdx = firstSection.IndexOf(" ", fieldIdx + 1);

            var field = firstSection.Substring(0, fieldIdx);
            var operatorName = firstSection.Substring(fieldIdx + 1, operatorIdx - fieldIdx - 1);
            var stringValue = firstSection.Substring(operatorIdx + 1);

            var propery = typeof(T)
                .GetProperties()
                .Where(r => r.Name.ToLower() == field.ToLower())
                .SingleOrDefault();

            if (propery == null)
            {
                return data;
            }

            object value = Cast(stringValue, propery.PropertyType);

            if (value == null)
            {
                return data; 
            }

            Expression<Func<T, bool>> query = CreatePredicate<T>(operatorName, field, value);

            if (query == null)
            {
                return data;
            }

            return data.Where(query);
        }
    }
}
