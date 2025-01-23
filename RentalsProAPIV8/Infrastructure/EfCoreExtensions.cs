using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RentalsProAPIV8.Infrastructure
{
    public static class EfCoreExtensions
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TrimCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static async Task BulkInsertAsync(this DbContext context, Type type, DataTable items)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (items == null || items.Rows.Count == 0) throw new ArgumentException("Items cannot be null or empty", nameof(items));

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            using var copy = new SqlBulkCopy(connection)
            {
                DestinationTableName = $"dbo.{type.Name}"
            };

            foreach (var prop in GetMappedProperties(type))
            {
                copy.ColumnMappings.Add(prop.Name, prop.Name);
            }

            await copy.WriteToServerAsync(items);
        }

        public static void BulkInsert(this DbContext context, Type type, DataTable items)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (items == null || items.Rows.Count == 0) throw new ArgumentException("Items cannot be null or empty", nameof(items));

            using var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            using var copy = new SqlBulkCopy(connection)
            {
                DestinationTableName = $"dbo.{type.Name}"
            };

            foreach (var prop in GetMappedProperties(type))
            {
                copy.ColumnMappings.Add(prop.Name, prop.Name);
            }

            copy.WriteToServer(items);
        }

        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T entity) where T : class
        {
            if (dbSet == null) throw new ArgumentNullException(nameof(dbSet));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = dbSet.GetContext();
            var entry = context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            entry.State = entry.IsKeySet() ? EntityState.Modified : EntityState.Added;
        }

        public static async Task<IList<T>> WhereAsync<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
        {
            if (dbSet == null) throw new ArgumentNullException(nameof(dbSet));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await dbSet.Where(predicate).ToListAsync();
        }

        private static DbContext GetContext<T>(this DbSet<T> dbSet) where T : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            return serviceProvider.GetService<ICurrentDbContext>().Context;
        }

        private static bool IsKeySet<T>(this EntityEntry<T> entry) where T : class
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null) return false;

            return key.Properties.All(property =>
            {
                var value = entry.Property(property.Name).CurrentValue;
                return value != null && !value.Equals(property.ClrType.IsValueType ? Activator.CreateInstance(property.ClrType) : null);
            });
        }

        private static PropertyInfo[] GetMappedProperties(Type type)
        {
            return TrimCache.GetOrAdd(type, t =>
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                 .Where(p => p.CanRead && p.CanWrite && !p.GetMethod.IsVirtual)
                 .ToArray());
        }
    }
}
