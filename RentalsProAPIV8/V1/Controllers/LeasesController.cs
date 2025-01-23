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
    public class LeasesController : CustomController
    {
        public LeasesController(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork) { }

        [HttpPut("UpdateLease")]
        public async Task<IActionResult> UpdateLease([FromBody] LeaseDTO LeaseDTO)
        {
            await _unitOfWork.LeaseRepo.UpdateLease(LeaseDTO);
            return Ok(true);
        }

        [HttpPost("PostForLease")]
        public async Task<IActionResult> PostForLease([FromBody] PostForLeaseParameters Parameters)
        {
            var leaseDTO = await _unitOfWork.LeaseRepo.GetLeaseDTO(Parameters.PropertyID, Parameters.UnitID);
            if (leaseDTO != null)
            {
                leaseDTO.Tenants = await _unitOfWork.UserRepo.GetLeaseUsers(leaseDTO.ID.Value);
            }

            return Ok(leaseDTO);
        }

        [HttpPost("PostLease")]
        public async Task<IActionResult> PostLease([FromBody] LeaseDTO LeaseDTO)
        {
            if (LeaseDTO == null)
            {
                return BadRequest("Invalid Lease Data.");
            }

            if (!LeaseDTO.PropertyID.HasValue)
            {
                return BadRequest("Missing required property.");
            }

            var newLease = ConvertDtoToDb(LeaseDTO);
            newLease.Active = true;
            await _unitOfWork.LeaseRepo.AddAsync(newLease);

            if (LeaseDTO.UnitID.HasValue)
            {
                await UpdateUnitStatus(LeaseDTO.UnitID.Value);
            }
            else if (LeaseDTO.PropertyID.HasValue)
            {
                await UpdatePropertyStatus(LeaseDTO.PropertyID.Value);
            }

            // Batch insert tenants with LeaseID set
            await AddTenantsAsync(newLease.ID, LeaseDTO.Tenants);

            return Ok(true);
        }

        private async Task UpdateUnitStatus(int unitID)
        {
            var unit = await _unitOfWork.UnitRepo.GetUnitAsync(unitID);
            if (unit != null)
            {
                unit.StatusID = (int)Enums.PropertyStatus.Rented;
                unit.PaymentStatusID = (int)Enums.PaymentStatus.Paid;
                unit.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.UnitRepo.UpdateAsync(unit);
            }
        }

        private async Task UpdatePropertyStatus(int propertyID)
        {
            var property = await _unitOfWork.PropertyRepo.GetPropertyAsync(propertyID);
            if (property != null)
            {
                property.StatusID = (int)Enums.PropertyStatus.Rented;
                property.PaymentStatusID = (int)Enums.PaymentStatus.Paid;
                property.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.PropertyRepo.UpdateAsync(property);
            }
        }

        private async Task AddTenantsAsync(int leaseId, IEnumerable<UserDTO> tenants)
        {
            var tenantUsers = tenants.Select(t =>
            {
                var tenant = ConvertDTOToDB(t);
                tenant.LeaseID = leaseId;
                return tenant;
            }).ToList();

            await _unitOfWork.UserRepo.PostUsers(tenantUsers); // Assumes batch insert capability
        }

        protected User ConvertDTOToDB(UserDTO userDTO) => new User
        {
            LeaseID = userDTO.LeaseID,
            CompanyID = userDTO.CompanyID,
            Username = userDTO.Username,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            Phone = userDTO.Phone.PhoneNumberForStorage(),
            Role = userDTO.Role,
            Birthdate = userDTO.Birthdate,
            CreatedDate = userDTO.CreatedDate ?? DateTime.UtcNow,
            UpdatedDate = userDTO.UpdatedDate,
            Active = userDTO.Active,
            IsOwner = userDTO.IsOwner.GetValueOrDefault(false)
        };

        protected Lease ConvertDtoToDb(LeaseDTO leaseDTO) => new Lease
        {
            PropertyID = leaseDTO.PropertyID.Value,
            UnitID = leaseDTO.UnitID,
            StartDate = leaseDTO.StartDate,
            EndDate = leaseDTO.EndDate,
            RentAmount = leaseDTO.RentAmount,
            SecurityDeposit = leaseDTO.SecurityDeposit,
            PetDeposit = leaseDTO.PetDeposit,
            UpdatedDate = leaseDTO.UpdatedDate
        };
    }
}
