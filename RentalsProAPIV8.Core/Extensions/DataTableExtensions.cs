using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class DataTableExtensions
    {
        public static int ContainsRow(this DataTable table, DataRowView rowToCompare)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (rowToCompare == null) throw new ArgumentNullException(nameof(rowToCompare));

            return table.ContainsRow(rowToCompare.Row);
        }

        public static int ContainsRow(this DataTable table, DataRow rowToCompare)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (rowToCompare == null) throw new ArgumentNullException(nameof(rowToCompare));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Columns.Cast<DataColumn>().All(col => table.Rows[i][col].Equals(rowToCompare[col])))
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool Contains(this DataTable table, string columnName, object valueToFind)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

            return table.AsEnumerable().Any(row => valueToFind.Equals(row[columnName]));
        }

        public static List<T> GetColumnValues<T>(this DataTable table, string columnName)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

            return table.AsEnumerable()
                        .Select(row => (T)Convert.ChangeType(row[columnName], typeof(T)))
                        .ToList();
        }

        public static List<string> GetColumnValues(this DataTable table, string columnName, bool trim = true)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

            return table.AsEnumerable()
                        .Select(row => trim ? row[columnName].ToString().Trim() : row[columnName].ToString())
                        .ToList();
        }

        public static T[] GetColumnValuesAsArray<T>(this DataTable table, string columnName)
        {
            return table.GetColumnValues<T>(columnName).ToArray();
        }

        public static string[] GetColumnValuesAsArray(this DataTable table, string columnName, bool trim = true)
        {
            return table.GetColumnValues(columnName, trim).ToArray();
        }

        public static string GetValuesJoined(this DataRow row, string separator, params string[] columnNames)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (columnNames == null || columnNames.Length == 0) throw new ArgumentNullException(nameof(columnNames));

            return string.Join(separator, columnNames.Select(columnName => row[columnName]?.ToString() ?? string.Empty));
        }

        public static List<Dictionary<string, object>> ToDictionaryList(this DataTable table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            return table.AsEnumerable()
                        .Select(row => table.Columns.Cast<DataColumn>()
                            .ToDictionary(col => col.ColumnName, col => row[col]))
                        .ToList();
        }

        public static string ToDelimitedString(this DataTable table, string delimiter = ",")
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException(nameof(delimiter));

            var builder = new StringBuilder();

            // Header row
            builder.AppendLine(string.Join(delimiter, table.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));

            // Data rows
            foreach (DataRow row in table.Rows)
            {
                builder.AppendLine(string.Join(delimiter, table.Columns.Cast<DataColumn>().Select(col => row[col]?.ToString() ?? string.Empty)));
            }

            return builder.ToString();
        }

        public static DataTable Filter(this DataTable table, Func<DataRow, bool> predicate)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var filteredTable = table.Clone(); // Copies the structure
            foreach (var row in table.AsEnumerable().Where(predicate))
            {
                filteredTable.ImportRow(row);
            }
            return filteredTable;
        }

        public static void AddRow(this DataTable table, params object[] values)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var newRow = table.NewRow();
            newRow.ItemArray = values;
            table.Rows.Add(newRow);
        }

        public static IEnumerable<DataRow> FindRows(this DataTable table, string columnName, object valueToFind)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

            return table.AsEnumerable().Where(row => valueToFind.Equals(row[columnName]));
        }

        public static IEnumerable<T> DistinctValues<T>(this DataTable table, string columnName)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException(nameof(columnName));

            return table.AsEnumerable()
                        .Select(row => row.Field<T>(columnName))
                        .Distinct();
        }
    }
}
