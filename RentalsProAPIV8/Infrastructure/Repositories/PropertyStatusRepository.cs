using Microsoft.EntityFrameworkCore;
using RentalsProAPIV8.Client.DataTransferObjects;
using System.Linq.Expressions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class PropertyStatusRepository : Repository<PropertyStatus>, IPropertyStatusRepository
    {
        public PropertyStatusRepository(RentalsProContext context) : base(context) { }

        public async Task<List<StatusDTO>> GetStatuses()
        {
            return await _context.PropertyStatuses.AsNoTracking().Select(MapToStatusDTO).ToListAsync();
        }

        private static Expression<Func<PropertyStatus, StatusDTO>> MapToStatusDTO => t => new StatusDTO(t.ID, t.Name, t.Name);
    }
}
