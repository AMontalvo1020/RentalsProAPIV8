using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface IPropertyRepository : IRepository<Property>
    {
        Task<Property> GetPropertyAsync(int propertyID);
        Task<PropertyDTO> GetPropertyDTOAsync(int propertyID);
        Task<List<PropertyDTO>> GetPropertyDTOsAsync(PostForPropertiesParameters parms);
        Task<Property?> GetPropertyByAddressAsync(string address);
        Task<int> UpdatePropertyAsync(PropertyDTO propertyDTO);
        Task<int> PatchStatusAsync(int propertyID, int statusID);
        Task<int> PatchPaymentStatusAsync(int propertyID, int statusID);
    }
}
