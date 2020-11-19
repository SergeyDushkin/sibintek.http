using System;

namespace sibintek.http.context
{
    public static partial class ODataExtension
    {
        private static object Cast(string obj, Type type)
        {
            object result;

            switch (type.Name)
            {
                case nameof(DateTime):
                    result = DateTime.Parse(obj);
                    break;
                case nameof(Guid):
                    result = Guid.Parse(obj);
                    break;
                default:
                    result = Convert.ChangeType(obj, type);
                    break;
            }

            return result;
        }
    }
}
