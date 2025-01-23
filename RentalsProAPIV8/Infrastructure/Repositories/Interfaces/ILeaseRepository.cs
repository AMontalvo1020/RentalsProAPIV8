using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface ILeaseRepository : IRepository<Lease>
    {
        Task<Lease?> GetLease(int? propertyID, int? unitID);
        Task<LeaseDTO?> GetLeaseDTO(int? propertyID, int? unitID);
        Task UpdateLease(LeaseDTO leaseDTO);
        Task PatchLeaseStatus(int? propertyID, int? unitID, bool isActive);
    }
}
