using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class StringExtensions
    {
        private static readonly HashSet<char> InvalidChars = new() { '-', '(', ')', ' ', ':', '.' };
        private static readonly string[] DateParseFormats = { "yyyy-MM-dd", "yyyy-M-d", "yyyyMMdd", "yyyyMd", "yyyy/MM/dd", "yyyy/M/d", "yyMMdd" };

        public static T EnumParse<T>(this string value, bool ignoreCase = true) where T : struct
        {
            if (value.IsEmpty()) throw new ArgumentException("Value cannot be empty.", nameof(value));

            return Enum.TryParse(value.Trim(), ignoreCase, out T result) ? result : throw new ArgumentException($"Invalid enum value for {typeof(T).Name}: {value}");
        }

        public static string ParseAddress(this string address)
        {
            if (address.IsEmpty()) throw new ArgumentException("Address cannot be empty.");

            return Regex.Replace(address, @"[^a-zA-Z0-9\s]", "").Replace(" ", "-").ToLower();
        }

        public static string AddSpacesAtCamel(this string value)
        {
            if (value.IsEmpty() || value.Length < 2) return value;

            var result = new StringBuilder(value.Replace('_', ' '));

            for (int i = 1; i < result.Length - 1; i++)
            {
                if (char.IsUpper(result[i]) && (char.IsLower(result[i - 1]) || char.IsLower(result[i + 1])) ||
                    char.IsDigit(result[i]) && char.IsLetter(result[i - 1]))
                {
                    result.Insert(i, ' ');
                    i++;
                }
            }

            return result.ToString();
        }

        public static string ToTitleCase(this string value) => value.IsEmpty() ? value : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());

        public static string ValueIfEmpty(this string input, string value, bool trim = true) => input.IsEmpty() ? value : (trim ? input.Trim() : input);

        public static string Prepend(this string source, string text) => text + source;
        public static string Append(this string source, string text) => source + text;

        public static string Left(this string input, int length) => input?.Substring(0, Math.Min(length, input.Length));
        public static string Right(this string input, int length) => input?.Substring(Math.Max(0, input.Length - length));

        public static string RemoveAccents(this string input) =>
            Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(input));

        public static string Repeat(this string input, int times) =>
            new StringBuilder().Insert(0, input, times).ToString();

        public static DateTime? DateParse(this string input)
        {
            return DateTime.TryParse(input, out var dt) || DateTime.TryParseExact(input, DateParseFormats, null, DateTimeStyles.None, out dt) ? dt : (DateTime?)null;
        }

        public static string CleanPath(this string input) => Path.GetInvalidFileNameChars().Aggregate(input, (current, c) => current.Replace(c.ToString(), string.Empty));

        public static bool IsEmpty(this string input) => string.IsNullOrWhiteSpace(input);

        public static string Truncate(this string value, int maxLength, bool addEllipsis = false)
        {
            if (value.IsEmpty() || maxLength <= 0) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - (addEllipsis ? 3 : 0)) + (addEllipsis ? "..." : string.Empty);
        }

        public static string Replace(this string input, string[] valuesToReplace, string replacement) => string.Join(replacement, input.Split(valuesToReplace, StringSplitOptions.None));

        public static StringBuilder Replace(this StringBuilder input, string[] valuesToReplace, string replacement)
        {
            foreach (var value in valuesToReplace) input.Replace(value, replacement);
            return input;
        }

        public static string Replace(this string input, char[] valuesToReplace, char replacement) => valuesToReplace.Aggregate(input, (current, value) => current.Replace(value, replacement));

        public static StringBuilder Replace(this StringBuilder input, char[] valuesToReplace, char replacement)
        {
            foreach (var value in valuesToReplace) input.Replace(value, replacement);
            return input;
        }

        // Phone Number Methods
        public static string PhoneNumberForStorage(this string phone) => string.IsNullOrEmpty(phone) ? string.Empty : new string(phone.Where(c => !InvalidChars.Contains(c)).ToArray());

        public static string PhoneNumberForDisplay(this string phone)
        {
            if (phone.IsEmpty()) return string.Empty;

            var cleaned = phone.PhoneNumberForStorage();
            if (cleaned.Length is 7 or 10 or 11)
            {
                cleaned = cleaned.Insert(cleaned.Length - 4, "-");
                if (cleaned.Length > 10) cleaned = $"({cleaned[..3]}) {cleaned[3..]}";
            }
            return cleaned;
        }
    }
}
