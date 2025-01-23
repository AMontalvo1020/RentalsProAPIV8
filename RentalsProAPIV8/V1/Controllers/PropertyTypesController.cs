﻿using Asp.Versioning;
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
    public class PropertyTypesController : CustomController
    {
        public PropertyTypesController(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork) { }

        [HttpGet("GetPropertyTypes")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var typeDTOs = await _unitOfWork.PropertyTypeRepo.GetPropertyTypes();
            return Ok(typeDTOs);
        }
    }
}
