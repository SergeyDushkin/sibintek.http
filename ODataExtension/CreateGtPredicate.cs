using System;
using System.Linq.Expressions;

namespace sibintek.http.context
{
    public static partial class ODataExtension
    {
        public static Expression<Func<T, bool>> CreateGtPredicate<T>(string field, object value)
        {
            var paramObject = Expression.Parameter(typeof(T), "p");
            var paramType = Expression.TypeAs(paramObject, typeof(T));
            var propField = Expression.Property(paramType, field);
            var constValue = Expression.Constant(value);

            var lamdaBody = Expression.GreaterThan(propField, constValue);

            Expression<Func<T, bool>> lamda = Expression.Lambda<Func<T, bool>>(lamdaBody, paramObject);

            return lamda;
        }
    }
}
