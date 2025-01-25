using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalsProAPIV8.Client.Constants;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;
using RentalsProAPIV8.Models;
using RentalsProAPIV8.V1.Controllers.Base;

namespace RentalsProAPIV8.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class PropertiesController : CustomController
    {
        public PropertiesController(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork) { }

        [HttpGet("GetProperty")]
        public async Task<IActionResult> GetProperty(int propertyID)
        {
            var propertyDTO = await _unitOfWork.PropertyRepo.GetPropertyDTOAsync(propertyID);
            if (propertyDTO == null)
                return NotFound(new { Message = "Property not found." });

            if (propertyDTO.OwnerID.HasValue)
                propertyDTO.Owner = await _unitOfWork.UserRepo.GetUserDTO(propertyDTO.OwnerID.Value);

            if (propertyDTO.CompanyID.HasValue)
                propertyDTO.Company = await _unitOfWork.CompanyRepo.GetCompanyDTO(propertyDTO.CompanyID.Value);

            if (propertyDTO.TypeID == (int)Enums.PropertyType.Commercial)
                propertyDTO.Units = await _unitOfWork.UnitRepo.GetUnitDTOs(propertyDTO.ID.Value);

            return Ok(propertyDTO);
        }

        [HttpPut("UpdateProperty")]
        public async Task<IActionResult> UpdateProperty([FromBody] PropertyDTO propertyDTO)
        {
            if (propertyDTO?.ID == null)
                return BadRequest("Invalid property data.");

            var updated = await _unitOfWork.PropertyRepo.UpdatePropertyAsync(propertyDTO);
            return updated > 0 ? Ok(new { Message = "Property updated successfully." })
                               : StatusCode(500, "Failed to update the property.");
        }

        [HttpPatch("PatchPropertyStatus")]
        public async Task<IActionResult> PatchPropertyStatus(int PropertyID, int StatusID)
        {
            var updated = await _unitOfWork.PropertyRepo.PatchStatusAsync(PropertyID, StatusID);

            // Check if the lease needs to be deactivated
            if (StatusID == (int)Enums.PropertyStatus.Rented)
            {
                await _unitOfWork.PropertyRepo.PatchPaymentStatusAsync(PropertyID, (int)Enums.PaymentStatus.Paid);
            }
            else if (new[] { (int)Enums.PropertyStatus.Vacant, (int)Enums.PropertyStatus.MoveOut, (int)Enums.PropertyStatus.Inactive }.Contains(StatusID))
            {
                var lease = await _unitOfWork.LeaseRepo.GetLease(PropertyID, null);
                if (lease != null)
                {
                    await Task.WhenAll(
                        _unitOfWork.LeaseRepo.PatchLeaseStatus(PropertyID, null, false),
                        _unitOfWork.UserRepo.BatchUpdateUserStatus(lease.ID, false)
                    );
                }
            }

            return Ok(updated.ToString());
        }

        [HttpPatch("PatchPropertyPaymentStatus")]
        public async Task<IActionResult> PatchPropertyPaymentStatus(int PropertyID, int StatusID)
        {
            var updated = await _unitOfWork.PropertyRepo.PatchPaymentStatusAsync(PropertyID, StatusID);
            return Ok(updated.ToString());
        }

        [HttpPost("PostForProperties")]
        public async Task<IActionResult> PostForProperties([FromBody] PostForPropertiesParameters parameters)
        {
            var properties = await _unitOfWork.PropertyRepo.GetPropertyDTOsAsync(parameters);
            var unitIDs = properties.Select(p => p.ID.Value).ToList();
            var units = await _unitOfWork.UnitRepo.GetUnitDTOs(unitIDs);

            properties.ForEach(p =>
            {
                p.Units = units.Where(u => u.PropertyID == p.ID).ToList();
            });

            return Ok(properties);
        }

        [HttpPost("PostProperty")]
        public async Task<IActionResult> PostProperty([FromBody] PropertyDTO PropertyDTO)
        {
            if (PropertyDTO == null)
                return BadRequest("Invalid property data.");

            var existingProperty = await _unitOfWork.PropertyRepo.GetPropertyByAddressAsync(PropertyDTO.Address.Address);
            if (existingProperty != null)
                return Conflict(new { Message = "A property with this address already exists." });

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newProperty = MapToProperty(PropertyDTO);
                await _unitOfWork.PropertyRepo.AddAsync(newProperty);

                if (PropertyDTO.Units?.Any() == true)
                {
                    PropertyDTO.Units.ForEach(unit => unit.PropertyID = newProperty.ID);
                    await _unitOfWork.UnitRepo.PostUnits(PropertyDTO.Units);
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { Message = "Property created successfully." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        private Property MapToProperty(PropertyDTO PropertyDTO)
        {
            return new Property
            {
                OwnerID = PropertyDTO.OwnerID,
                CompanyID = PropertyDTO.CompanyID,
                StatusID = PropertyDTO.StatusID,
                TypeID = PropertyDTO.TypeID,
                PaymentStatusID = PropertyDTO.PaymentStatusID,
                Address = PropertyDTO.Address.Address,
                City = PropertyDTO.Address.City,
                State = PropertyDTO.Address.State,
                ZipCode = PropertyDTO.Address.ZipCode,
                Bedrooms = PropertyDTO.Bedrooms,
                Bathrooms = PropertyDTO.Bathrooms,
                Size = PropertyDTO.Size,
                Amenities = PropertyDTO.Amenities,
                PurchaseDate = PropertyDTO.PurchaseDate.Value,
                PurchasePrice = PropertyDTO.PurchasePrice,
                Active = PropertyDTO.Active // Default to true if null
            };
        }
    }
}
