using System.Linq.Expressions;
using System.Reflection;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class GenericTypeExtensions
    {
        // Gets the property name from a property expression.
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression?.Body is not MemberExpression memberExpression)
            {
                throw new ArgumentException("Expression must be a property expression", nameof(propertyExpression));
            }

            return memberExpression.Member.Name;
        }

        // Converts an object to a specified type.
        public static T As<T>(this object source)
        {
            if (source is null) return default;  // Handling null inputs gracefully.
            return (T)Convert.ChangeType(source, typeof(T));
        }

        // Checks if a value is present in a collection.
        public static bool In<T>(this T input, params T[] values)
        {
            return values?.Contains(input) ?? false;
        }

        // Returns the first element matching a predicate or a default value.
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return source.FirstOrDefault(predicate) ?? defaultValue;
        }

        // Checks if a type is a primitive type.
        public static bool IsPrimitiveType(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return type.IsPrimitive || type.IsEnum || type == typeof(decimal) || type == typeof(string) || type == typeof(DateTime);
        }

        // Checks if a type is a text type (char or string).
        public static bool IsTextType(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return type == typeof(char) || type == typeof(string);
        }

        // Checks if a type is a decimal type.
        public static bool IsDecimalType(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return type == typeof(decimal);
        }

        // Converts a DB object to a DTO object using reflection.
        public static T ConvertDBToDTO<T>(this object dbObject) where T : new()
        {
            if (dbObject is null) throw new ArgumentNullException(nameof(dbObject));

            var dto = new T();
            var dbObjectType = dbObject.GetType();
            var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.CanWrite)
                {
                    var dbProperty = dbObjectType.GetProperty(dtoProperty.Name);
                    if (dbProperty?.CanRead == true)
                    {
                        var value = dbProperty.GetValue(dbObject);
                        // Only set the value if it's not null (optimization for null-checking)
                        if (value != null)
                        {
                            dtoProperty.SetValue(dto, value);
                        }
                    }
                }
            }

            return dto;
        }
    }
}
