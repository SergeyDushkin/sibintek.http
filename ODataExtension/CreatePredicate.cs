using System;
using System.Linq.Expressions;

namespace sibintek.http.context
{
    public static partial class ODataExtension
    {
        private static Expression<Func<T, bool>> CreatePredicate<T>(string _, string field, object value)
        {
            Expression<Func<T, bool>> predicate = null;

            switch(_.ToLower())
            {
                case "eq":
                    predicate = CreateEqPredicate<T>(field, value);
                    break;
                case "ne":
                    predicate = CreateNePredicate<T>(field, value);
                    break;
                case "gt":
                    predicate = CreateGtPredicate<T>(field, value);
                    break;
                case "ge":
                    predicate = CreateGePredicate<T>(field, value);
                    break;
                case "lt":
                    predicate = CreateLtPredicate<T>(field, value);
                    break;
                case "le":
                    predicate = CreateLePredicate<T>(field, value);
                    break;
                default:
                    break;
            }

            return predicate;
        }
    }
}
