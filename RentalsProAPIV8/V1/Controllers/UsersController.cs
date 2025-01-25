using System.Security.Cryptography;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;
using RentalsProAPIV8.V1.Controllers.Base;

namespace RentalsProAPIV8.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class UsersController : CustomController
    {
        public UsersController(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork) { }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(int UserID)
        {
            var userDTO = await _unitOfWork.UserRepo.GetUserDTO(UserID);
            return Ok(userDTO);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest(new { Message = "Invalid user data." });

            await _unitOfWork.UserRepo.UpdateUser(userDTO);
            return Ok(new { Message = "User updated successfully." });
        }

        [HttpPost("PostForValidUser")]
        public async Task<IActionResult> PostForValidUser([FromBody] ValidUserParameters parameters)
        {
            if (parameters == null)
                return BadRequest(new UserDTO());

            var userDTO = await _unitOfWork.UserRepo.GetUserDTO(parameters.Username);
            if (userDTO == null || !ValidatePassword(userDTO, parameters.Password))
                return Unauthorized(new UserDTO());

            return Ok(userDTO);
        }

        [HttpPost("PostForUsers")]
        public async Task<IActionResult> PostForUsers([FromBody] PostForUserParameters parameters)
        {
            if (parameters == null)
                return BadRequest(new List<UserDTO>());

            var userDTOs = await _unitOfWork.UserRepo.GetUsersAsync(parameters);
            if (userDTOs == null || userDTOs.Count == 0)
                return NotFound(new List<UserDTO>());

            return Ok(userDTOs);
        }

        private bool ValidatePassword(UserDTO user, string attemptedPassword)
        {
            ValidatePreconditions(user, attemptedPassword);

            if (user.PasswordHash.IsEmpty() || user.PasswordSalt.IsEmpty())
                throw new ArgumentException("Stored password hash or salt is invalid.");

            var attemptedHash = Hashing.ComputeHash(attemptedPassword, user.PasswordSalt, HashAlgorithmType.PBKDF2);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(attemptedHash),
                Convert.FromBase64String(user.PasswordHash)
            );
        }

        private static void ValidatePreconditions(UserDTO user, string attemptedPassword)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(attemptedPassword))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(attemptedPassword));
        }
    }
}
