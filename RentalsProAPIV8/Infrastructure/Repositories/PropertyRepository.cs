using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        public PropertyRepository(RentalsProContext context) : base(context) { }

        public async Task<Property> GetPropertyAsync(int propertyID)
        {
            return await _context.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.ID == propertyID);
        }

        public async Task<PropertyDTO> GetPropertyDTOAsync(int propertyID)
        {
            return await _context.Properties.AsNoTracking().Where(p => p.ID == propertyID).Select(MapToPropertyDTO).SingleOrDefaultAsync();
        }

        public async Task<List<PropertyDTO>> GetPropertyDTOsAsync(PostForPropertiesParameters parms)
        {
            var query = _context.Properties.AsNoTracking();

            // Apply filters conditionally
            if (parms.UserID.HasValue)
                query = query.Where(p => p.OwnerID == parms.UserID.Value);
            if (parms.CompanyID.HasValue)
                query = query.Where(p => p.CompanyID == parms.CompanyID.Value);
            if (!parms.Address.IsEmpty())
                query = query.Where(p => EF.Functions.Like(p.Address, $"%{parms.Address}%"));
            if (parms.PaymentStatus.HasValue)
                query = query.Where(p => p.PaymentStatusID == parms.PaymentStatus.Value);
            if (parms.Status?.Any() == true)
                query = query.Where(p => parms.Status.Contains(p.StatusID));
            if (parms.Type?.Any() == true)
                query = query.Where(p => parms.Type.Contains(p.TypeID));

            // Project directly to DTO to minimize data loading
            return await query.Select(MapToPropertyDTO)
                              .ToListAsync();
        }

        public async Task<Property?> GetPropertyByAddressAsync(string address)
        {
            return await _dataSet.FirstOrDefaultAsync(p => EF.Functions.Like(p.Address, $"%{address}%"));
        }

        public async Task<int> UpdatePropertyAsync(PropertyDTO propertyDTO)
        {
            var property = await _context.Properties.FirstAsync(p => p.ID == propertyDTO.ID);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {propertyDTO.ID} not found.");

            if (property.Address != propertyDTO.Address.Address)
                property.Address = propertyDTO.Address.Address;
            if (property.Bedrooms != propertyDTO.Bedrooms)
                property.Bedrooms = propertyDTO.Bedrooms;
            if (property.Bathrooms != propertyDTO.Bathrooms)
                property.Bathrooms = propertyDTO.Bathrooms;
            if (property.Size != propertyDTO.Size)
                property.Size = propertyDTO.Size ?? property.Size;
            if (property.PurchasePrice != propertyDTO.PurchasePrice)
                property.PurchasePrice = propertyDTO.PurchasePrice;
            if (property.PurchaseDate != propertyDTO.PurchaseDate)
                property.PurchaseDate = propertyDTO.PurchaseDate.Value;
            if (property.Active != propertyDTO.Active)
                property.Active = propertyDTO.Active;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> PatchStatusAsync(int propertyID, int statusID)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.ID == propertyID);

            if (property == null) throw new KeyNotFoundException($"Property with ID {propertyID} not found.");

            property.StatusID = statusID;
            property.UpdatedDate = DateTime.UtcNow;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> PatchPaymentStatusAsync(int propertyID, int statusID)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.ID == propertyID);

            if (property == null) throw new KeyNotFoundException($"Property with ID {propertyID} not found.");

            property.PaymentStatusID = statusID;
            property.UpdatedDate = DateTime.UtcNow;

            return await _context.SaveChangesAsync();
        }

        private static readonly Expression<Func<Property, PropertyDTO>> MapToPropertyDTO = p => new PropertyDTO
        {
            ID = p.ID,
            CompanyID = p.CompanyID,
            OwnerID = p.OwnerID,
            Address = new AddressDTO(p.Address, p.City, p.State, p.ZipCode),
            Bedrooms = p.Bedrooms,
            Bathrooms = p.Bathrooms,
            Size = p.Size,
            PurchasePrice = p.PurchasePrice,
            PurchaseDate = p.PurchaseDate,
            StatusID = p.StatusID,
            Status = new StatusDTO(p.Status.ID, p.Status.Name, p.Status.Color),
            PaymentStatusID = p.PaymentStatusID,
            PaymentStatus = new StatusDTO(p.PaymentStatus.ID, p.PaymentStatus.Name, p.PaymentStatus.Color),
            TypeID = p.TypeID,
            Type = new TypeDTO(p.Type.ID, p.Type.Name),
            Active = p.Active,
            CreatedDate = p.CreatedDate,
            UpdatedDate = p.UpdatedDate
        };
    }
}
