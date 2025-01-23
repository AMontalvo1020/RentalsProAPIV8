using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T>, IAsyncDisposable where T : class
    {
        protected readonly RentalsProContext _context;
        protected readonly DbSet<T> _dataSet;

        protected Repository(RentalsProContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dataSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll() => _dataSet.AsNoTracking();

        public async Task<int> AddAsync(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            await _dataSet.AddAsync(item);
            return await SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> items)
        {
            if (items == null || !items.Any()) throw new ArgumentException("Collection cannot be null or empty.", nameof(items));
            await _dataSet.AddRangeAsync(items);
            return await SaveChangesAsync();
        }

        public async Task BulkInsertAsync(IEnumerable<T> items)
        {
            if (items == null || !items.Any()) throw new ArgumentException("Collection cannot be null or empty.", nameof(items));
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.BulkInsertAsync(typeof(T), items.ToDataTable());
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"Bulk insert operation failed: {ex.Message}", ex);
            }
            catch (TimeoutException ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"Bulk insert operation timed out: {ex.Message}", ex);
            }
        }

        public async Task<int> UpdateAsync(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _dataSet.Update(item);
            return await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Concurrency issue detected.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Database update error.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while saving changes.", ex);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
