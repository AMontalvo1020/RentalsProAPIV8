using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<CompanyDTO?> GetCompanyDTO(int? companyID);
        Task<List<CompanyDTO?>> GetCompanyDTOs(List<int> companyIDs);
    }
}
