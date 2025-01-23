using System.ComponentModel.Design;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(RentalsProContext context) : base(context) { }

        public async Task<CompanyDTO?> GetCompanyDTO(int? companyID)
        {
            return await _context.Companies.Where(c => c.ID == companyID).Select(MapToDTO).FirstOrDefaultAsync();
        }

        public async Task<List<CompanyDTO?>> GetCompanyDTOs(List<int> companyIDs)
        {
            return await _context.Companies.Where(c => c.ID.In(companyIDs)).Select(MapToDTO).ToListAsync();
        }

        private static Expression<Func<Company, CompanyDTO>> MapToDTO => c => new CompanyDTO
        {
            ID = c.ID,
            Name = c.Name,
            Address = new AddressDTO(c.Address, c.City, c.State, c.ZipCode),
            Phone = c.Phone,
            Email = c.Email,
            CreatedDate = c.CreatedDate,
            UpdatedDate = c.UpdatedDate,
            Active = c.Active
        };
    }
}
