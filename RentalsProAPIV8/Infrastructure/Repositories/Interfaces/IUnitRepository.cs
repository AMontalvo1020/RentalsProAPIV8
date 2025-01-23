using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<Unit?> GetUnitAsync(int unitID);
        Task<UnitDTO> GetUnitDTO(int unitID);
        Task<List<UnitDTO>> GetUnitDTOs(int propertyID, bool? Active = null);
        Task<List<UnitDTO>> GetUnitDTOs(List<int> propertyIDs, bool? Active = null);
        Task<int> PatchStatusAsync(int unitID, int statusID);
        Task<int> PatchPaymentStatusAsync(int unitID, int statusID);
        Task<int> PostUnit(UnitDTO unit);
        Task PostUnits(List<UnitDTO> units);
    }
}
