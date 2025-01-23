using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class UnitRepository : Repository<Unit>, IUnitRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private static readonly string UnitCacheKeyPrefix = "Unit_";

        public UnitRepository(RentalsProContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;

        }

        public async Task<Unit?> GetUnitAsync(int unitID)
        {
            return await _context.Units.FindAsync(unitID);
        }

        public async Task<UnitDTO> GetUnitDTO(int unitID)
        {
            var cacheKey = $"{UnitCacheKeyPrefix}{unitID}";
            if (!_cache.TryGetValue(cacheKey, out UnitDTO? unitDTO))
            {
                unitDTO = await _context.Units.Where(u => u.ID == unitID).Select(MapToUnitDTO).FirstAsync();

                if (unitDTO != null)
                {
                    _cache.Set(cacheKey, unitDTO, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                }
            }
            return unitDTO;
        }

        public async Task<List<UnitDTO>> GetUnitDTOs(int propertyID, bool? Active = null)
        {
            return await _context.Units.Where(u => u.PropertyID == propertyID && (!Active.HasValue || u.Active == Active)).Select(MapToUnitDTO).ToListAsync();
        }

        public async Task<List<UnitDTO>> GetUnitDTOs(List<int> propertyIDs, bool? Active = null)
        {
            return await _context.Units.Where(u => propertyIDs.Contains(u.PropertyID) && (!Active.HasValue || u.Active == Active)).Select(MapToUnitDTO).ToListAsync();
        }

        public async Task<int> PatchStatusAsync(int unitID, int statusID)
        {
            var unit = await _context.Units.FirstAsync(p => p.PropertyID == unitID);
            unit.StatusID = statusID;
            unit.UpdatedDate = DateTime.UtcNow;
            return await UpdateAsync(unit);
        }

        public async Task<int> PatchPaymentStatusAsync(int unitID, int statusID)
        {
            var unit = await _context.Units.FirstAsync(p => p.PropertyID == unitID);
            unit.PaymentStatusID = statusID;
            unit.UpdatedDate = DateTime.UtcNow;
            return await UpdateAsync(unit);
        }

        public async Task<int> PostUnit(UnitDTO unit)
        {
            _context.Units.Add(ConvertDtoToDb(unit));
            return await _context.SaveChangesAsync();
        }

        public async Task PostUnits(List<UnitDTO> units)
        {
            await _context.BulkInsertAsync(typeof(Unit), units.Select(ConvertDtoToDb).ToDataTable());
        }

        // Reuse the mapping expression
        private static readonly Expression<Func<Unit, UnitDTO>> MapToUnitDTO = u => new UnitDTO
        {
            UnitID = u.ID, // Assuming UnitDTO has an ID property
            PropertyID = u.PropertyID,
            UnitNumber = u.UnitNumber,
            Floor = u.Floor,
            Bedrooms = u.Bedrooms,
            Bathrooms = u.Bathrooms,
            SquareFeet = u.SquareFeet,
            Utilities = u.Utilities,
            Furnished = u.Furnished,
            ParkingSpace = u.ParkingSpace,
            CreatedDate = u.CreatedDate,
            UpdatedDate = u.UpdatedDate,
            Active = u.Active,
            StatusID = u.StatusID,
            PaymentStatusID = u.PaymentStatusID,
            Status = new StatusDTO(u.Status.ID, u.Status.Name, u.Status.Color),
            PaymentStatus = new StatusDTO(u.PaymentStatus.ID, u.PaymentStatus.Name, u.PaymentStatus.Color),
        };

        private Unit ConvertDtoToDb(UnitDTO unitDTO) => new Unit
        {
            PropertyID = unitDTO.PropertyID,
            UnitNumber = unitDTO.UnitNumber,
            Floor = unitDTO.Floor,
            Bedrooms = unitDTO.Bedrooms,
            Bathrooms = unitDTO.Bathrooms,
            SquareFeet = unitDTO.SquareFeet,
            Utilities = unitDTO.Utilities,
            Furnished = unitDTO.Furnished,
            ParkingSpace = unitDTO.ParkingSpace,
            CreatedDate = unitDTO.CreatedDate,
            UpdatedDate = unitDTO.UpdatedDate,
            Active = unitDTO.Active,
            StatusID = unitDTO.StatusID,
            PaymentStatusID = unitDTO.PaymentStatusID
        };
    }
}
