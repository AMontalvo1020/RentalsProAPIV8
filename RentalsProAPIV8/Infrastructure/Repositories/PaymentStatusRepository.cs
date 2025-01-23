using Microsoft.EntityFrameworkCore;
using RentalsProAPIV8.Client.DataTransferObjects;
using System.Linq.Expressions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class PaymentStatusRepository : Repository<PaymentStatus>, IPaymentStatusRepository
    {
        public PaymentStatusRepository(RentalsProContext context) : base(context) { }

        public Task<List<StatusDTO>> GetStatuses()
        {
            return _context.PaymentStatuses.AsNoTracking().Select(MapToStatusDTO).ToListAsync();
        }

        private static Expression<Func<PaymentStatus, StatusDTO>> MapToStatusDTO => ps => new StatusDTO(ps.ID, ps.Name, ps.Color);
    }
}
