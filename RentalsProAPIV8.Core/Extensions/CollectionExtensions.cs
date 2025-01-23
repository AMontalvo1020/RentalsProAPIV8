using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class CollectionExtensions
    {
        // Collection and Array Utilities
        public static bool In<T>(this T obj, IEnumerable<T> collection) => collection?.Contains(obj) ?? false;

        public static bool IsEmpty(this Array data) => data == null || data.Length == 0;

        public static bool IsEmpty(this IList data) => data == null || data.Count == 0;

        public static bool IsEmpty(this IEnumerable data) => !data?.GetEnumerator().MoveNext() ?? true;

        // Pagination
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize) =>
            source?.Skip((pageNumber - 1) * pageSize).Take(pageSize) ?? Enumerable.Empty<T>();

        // ValueAt Methods
        public static TValue ValueAt<TValue>(this IList<TValue> collection, int index, TValue @default = default) =>
            (index >= 0 && index < collection?.Count) ? collection[index] : @default;

        public static TValue ValueAt<TValue>(this IEnumerable<TValue> enumerable, int index, TValue @default = default)
        {
            if (enumerable == null || index < 0)
                return @default;

            return enumerable.Skip(index).FirstOrDefault() ?? @default;
        }

        // SQL List String Methods
        public static string ToSqlListString(this IEnumerable values) =>
            values != null && values.Cast<object>().Any()
                ? $"('{string.Join("', '", values.Cast<object>())}')"
                : "( )";

        // Separated String Methods
        public static string ToSeparatedString(this Array values, string separator) =>
            string.Join(separator, values?.Cast<object>().Select(v => v?.ToString()) ?? Array.Empty<string>());

        public static string ToSeparatedString(this IList values, string separator) =>
            string.Join(separator, values?.Cast<object>().Select(v => v?.ToString()) ?? Array.Empty<string>());

        public static string ToSeparatedString<T>(this IEnumerable<T> values, string separator) =>
            string.Join(separator, values?.Select(v => v?.ToString()) ?? Array.Empty<string>());

        // Natural Language List
        public static string ToNaturalLanguageList<T>(this IEnumerable<T> values, bool oxfordComma = true)
        {
            var list = values?.ToList() ?? new List<T>();
            return list.Count switch
            {
                0 => string.Empty,
                1 => list[0]?.ToString(),
                2 => $"{list[0]} and {list[1]}",
                _ => $"{string.Join(", ", list.Take(list.Count - 1))}{(oxfordComma ? "," : string.Empty)} and {list.Last()}"
            };
        }

        // Conversion Methods
        public static ObservableCollection<T> AsObservable<T>(this IEnumerable<T> input) => new(input ?? Enumerable.Empty<T>());

        public static BindingList<T> AsBindingList<T>(this IEnumerable<T> input) => new(new List<T>(input ?? Enumerable.Empty<T>()));

        public static DataTable ToDataTable(this IEnumerable input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var firstItem = input.Cast<object>().FirstOrDefault();
            if (firstItem == null) throw new InvalidOperationException("The collection is empty.");

            var itemType = firstItem.GetType();

            return itemType switch
            {
                var type when type.IsPrimitive || type == typeof(string) => CreateDataTableForPrimitiveType(type, input),
                _ => CreateDataTableForObject(itemType, input)
            };
        }

        private static DataTable CreateDataTableForPrimitiveType(Type type, IEnumerable input)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Value", type);

            foreach (var item in input)
                dataTable.Rows.Add(item);

            return dataTable;
        }

        private static DataTable CreateDataTableForObject(Type itemType, IEnumerable input)
        {
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dataTable = new DataTable();

            foreach (var prop in properties)
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var obj in input)
            {
                var row = dataTable.NewRow();
                foreach (var prop in properties)
                    row[prop.Name] = prop.GetValue(obj) ?? DBNull.Value;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        // ObservableCollection Extensions
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                collection.Add(item);
        }

        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items.ToList()) // ToList prevents modification during enumeration
                collection.Remove(item);
        }

        public static void Replace<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (newItems == null) throw new ArgumentNullException(nameof(newItems));

            collection.Clear();
            collection.AddRange(newItems);
        }
    }
}
