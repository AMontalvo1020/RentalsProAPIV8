using RentalsProAPIV8.Client.DataTransferObjects;
using System.Linq.Expressions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;
using Microsoft.EntityFrameworkCore;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class PropertyTypeRepository : Repository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(RentalsProContext context) : base(context) { }

        public async Task<List<TypeDTO>> GetPropertyTypes()
        {
            return await _context.PropertyTypes.Select(MapToTypeDTO).AsNoTracking().ToListAsync();
        }

        private static Expression<Func<PropertyType, TypeDTO>> MapToTypeDTO => t => new TypeDTO(t.ID, t.Name);
    }
}
