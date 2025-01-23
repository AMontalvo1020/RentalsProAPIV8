using Microsoft.AspNetCore.Mvc;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;

namespace RentalsProAPIV8.V1.Controllers.Base
{
    public abstract class CustomController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IConfiguration _configuration;

        public CustomController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
    }
}
