using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;
using RentalsProAPIV8.V1.Controllers.Base;

namespace RentalsProAPIV8.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentStatusesController : CustomController
    {
        public PaymentStatusesController(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork) { }

        [HttpGet("GetPaymentStatuses")]
        public async Task<IActionResult> GetPaymentStatuses()
        {
            var statusDTOs = await _unitOfWork.PaymentStatusRepo.GetStatuses();
            return Ok(statusDTOs);
        }
    }
}
