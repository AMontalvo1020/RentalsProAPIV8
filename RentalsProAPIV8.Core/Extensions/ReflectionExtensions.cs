using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static object? GetProperty(this object obj, string propertyName, params object[] parameters)
        {
            return obj?.GetType().GetProperty(propertyName)?.GetValue(obj, parameters);
        }

        public static Dictionary<string, object> GetProperties(this object obj)
        {
            return obj?.GetType().GetProperties().ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null)) ?? [];
        }

        public static void SetProperty(this object obj, string propertyName, object value, params object[] index)
        {
            obj?.GetType().GetProperty(propertyName)?.SetValue(obj, value, index);
        }

        public static T? GetAttributeInstance<T>(this object obj, string propName) where T : Attribute
        {
            return obj?.GetType().GetProperty(propName)?.GetCustomAttributes(false)?.OfType<T>()?.FirstOrDefault();
        }
    }
}
