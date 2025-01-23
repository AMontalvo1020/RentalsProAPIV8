using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalsProAPIV8.Client.Constants;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;
using RentalsProAPIV8.Models;
using RentalsProAPIV8.V1.Controllers.Base;

namespace RentalsProAPIV8.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class UnitsController : CustomController
    {
        public UnitsController(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork) { }

        [HttpGet("GetUnit")]
        public async Task<IActionResult> GetUnit(int UnitID)
        {
            var unitDTO = await _unitOfWork.UnitRepo.GetUnitDTO(UnitID);

            if (unitDTO is null)
            {
                return NotFound(new UnitDTO());
            }
            else
            {
                return Ok(unitDTO);
            }
        }

        [HttpGet("GetUnits")]
        public async Task<IActionResult> GetUnits(int PropertyID, bool Active)
        {
            var unitDTOs = await _unitOfWork.UnitRepo.GetUnitDTOs(PropertyID, Active);

            if (unitDTOs is null || unitDTOs.Count == 0)
            {
                return NotFound(new List<UnitDTO>());
            }
            else
            {
                return Ok(unitDTOs);
            }
        }

        [HttpPatch("PatchUnitStatus")]
        public async Task<IActionResult> PatchUnitStatus(int UnitID, int StatusID)
        {
            var updated = await _unitOfWork.UnitRepo.PatchStatusAsync(UnitID, StatusID);
            // Check if additional processing is required for specific status IDs
            if (StatusID == (int)Enums.PropertyStatus.Vacant || StatusID == (int)Enums.PropertyStatus.MoveOut)
            {
                // Update lease and user status concurrently
                var lease = await _unitOfWork.LeaseRepo.GetLease(null, UnitID);
                if (lease != null)
                {
                    await _unitOfWork.LeaseRepo.PatchLeaseStatus(null, UnitID, false);
                    await _unitOfWork.UserRepo.BatchUpdateUserStatus(lease.ID, false);
                }
            }

            return Ok(updated);
        }

        [HttpPatch("PatchUnitPaymentStatus")]
        public async Task<IActionResult> PatchUnitPaymentStatus(int UnitID, int StatusID)
        {
            var updated = await _unitOfWork.UnitRepo.PatchPaymentStatusAsync(UnitID, StatusID);
            return Ok(updated);
        }

        [HttpPost("PostUnit")]
        public async Task<IActionResult> PostUnit([FromBody] UnitDTO UnitDTO)
        {
            var added = await _unitOfWork.UnitRepo.PostUnit(UnitDTO);
            return Ok(added);
        }

        [HttpPost("PostUnits")]
        public async Task<IActionResult> PostUnits([FromBody] List<UnitDTO> UnitDTOs)
        {
            await _unitOfWork.UnitRepo.PostUnits(UnitDTOs);
            return Ok(true);
        }
    }
}
