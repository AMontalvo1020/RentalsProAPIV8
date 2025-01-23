using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface IPropertyStatusRepository : IRepository<PropertyStatus>
    {
        Task<List<StatusDTO>> GetStatuses();
    }
}
