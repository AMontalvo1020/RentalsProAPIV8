namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<int> AddAsync(T item);
        Task<int> AddRangeAsync(IEnumerable<T> items);
        Task BulkInsertAsync(IEnumerable<T> items);
        Task<int> UpdateAsync(T item);
        Task<int> SaveChangesAsync();
    }
}
