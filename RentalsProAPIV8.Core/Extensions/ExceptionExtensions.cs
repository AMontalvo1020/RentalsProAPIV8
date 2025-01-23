using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class ExceptionExtensions
    {
        // Returns the full message of the exception, optionally including the stack trace.
        public static string GetFullMessage(this Exception ex, bool includeStackTrace = false)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            var sb = new StringBuilder(ex.Message);

            // Handle AggregateException and inner exceptions recursively
            AppendInnerExceptions(ex, sb);

            // Append stack trace if needed
            if (includeStackTrace)
            {
                sb.AppendLine().AppendLine().Append(ex.StackTrace);
            }

            return sb.ToString();
        }

        private static void AppendInnerExceptions(Exception ex, StringBuilder sb)
        {
            if (ex is AggregateException aggEx)
            {
                foreach (var innerEx in aggEx.InnerExceptions)
                {
                    AppendInnerExceptions(innerEx, sb);
                }
            }
            else if (ex.InnerException != null)
            {
                sb.AppendLine(ex.InnerException.GetFullMessage());
            }
        }

        // Flattens all inner exceptions (recursive)
        public static IEnumerable<Exception> FlattenExceptions(this Exception ex)
        {
            if (ex == null) yield break;

            if (ex is AggregateException aggEx)
            {
                foreach (var innerEx in aggEx.InnerExceptions)
                {
                    foreach (var flatEx in innerEx.FlattenExceptions())
                    {
                        yield return flatEx;
                    }
                }
            }
            else
            {
                yield return ex;
                if (ex.InnerException != null)
                {
                    foreach (var innerEx in ex.InnerException.FlattenExceptions())
                    {
                        yield return innerEx;
                    }
                }
            }
        }

        // Returns the deepest exception with its level in the hierarchy
        public static (Exception Exception, int Level) GetDeepestException(this Exception ex, int level = 0)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            if (ex is not AggregateException)
            {
                return (ex, level);
            }

            var deepestException = ex;
            var deepestLevel = level + 1;

            // Navigate through the inner exceptions
            while (deepestException.InnerException != null)
            {
                deepestException = deepestException.InnerException;
                deepestLevel++;
            }

            return (deepestException, deepestLevel);
        }

        // Get the deepest exception from a collection of exceptions
        public static (Exception Exception, int Level) GetDeepestException(this IEnumerable<Exception> exceptions, int level = 0)
        {
            if (exceptions == null) throw new ArgumentNullException(nameof(exceptions));

            var deepest = (Exception: (Exception)null, Level: 0);

            foreach (var ex in exceptions)
            {
                var (deepEx, deepLevel) = ex.GetDeepestException(level + 1);

                if (deepLevel > deepest.Level)
                {
                    deepest = (deepEx, deepLevel);
                }
            }

            return deepest;
        }
    }

    // Enum to categorize exceptions
    public enum ExceptionCategory
    {
        Error,
        Warning,
        Information
    }
}
