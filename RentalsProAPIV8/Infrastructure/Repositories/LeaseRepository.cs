using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class LeaseRepository : Repository<Lease>, ILeaseRepository
    {
        public LeaseRepository(RentalsProContext context) : base(context) { }

        public async Task<Lease?> GetLease(int? propertyID, int? unitID)
        {
            return await _context.Leases.FirstOrDefaultAsync(l => (!propertyID.HasValue || l.PropertyID == propertyID)
                                                               && (!unitID.HasValue || l.UnitID == unitID)
                                                               && l.Active);
        }

        public async Task<LeaseDTO?> GetLeaseDTO(int? propertyID, int? unitID)
        {
            return await _context.Leases.Where(l => (!propertyID.HasValue || l.PropertyID == propertyID)
                                                 && (!unitID.HasValue || l.UnitID == unitID)
                                                 && l.Active)
                                        .Select(MapToLeaseDTO)
                                        .FirstOrDefaultAsync();
        }

        public async Task UpdateLease(LeaseDTO leaseDTO)
        {
            // Fetch lease directly and handle potential null cases
            var lease = await _context.Leases.FirstOrDefaultAsync(l => l.ID == leaseDTO.ID) ?? throw new Exception($"{nameof(Lease)} not found for {nameof(Lease.ID)}");

            // Update only if values have changed
            if (lease.RentAmount != leaseDTO.RentAmount)
            {
                lease.RentAmount = leaseDTO.RentAmount;
            }
            if (lease.SecurityDeposit != leaseDTO.SecurityDeposit)
            {
                lease.SecurityDeposit = leaseDTO.SecurityDeposit;
            }
            if (lease.PetDeposit != leaseDTO.PetDeposit)
            {
                lease.PetDeposit = leaseDTO.PetDeposit;
            }

            // Save changes only if there are updates
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task PatchLeaseStatus(int? propertyID, int? unitID, bool isActive)
        {
            var lease = await _context.Leases.FirstOrDefaultAsync(l => (propertyID == null || l.PropertyID == propertyID)
                                                                    && (unitID == null || l.UnitID == unitID)
                                                                    && l.Active);
            if (lease == null || lease.Active == isActive) return;
            lease.Active = isActive;
            await _context.SaveChangesAsync();
        }

        private static Expression<Func<Lease, LeaseDTO>> MapToLeaseDTO => l => new LeaseDTO
        {
            ID = l.ID,
            PropertyID = l.PropertyID,
            UnitID = l.UnitID,
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            RentAmount = l.RentAmount,
            SecurityDeposit = l.SecurityDeposit,
            PetDeposit = l.PetDeposit,
            CreateDate = l.CreatedDate,
            UpdatedDate = l.UpdatedDate,
            IsActive = l.Active,
        };
    }
}
