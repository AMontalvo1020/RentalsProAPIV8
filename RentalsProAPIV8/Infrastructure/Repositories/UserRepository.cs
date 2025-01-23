using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(RentalsProContext context) : base(context) { }

        public async Task<UserDTO?> GetUserDTO(int userID)
        {
            return await _context.Users.Where(u => u.ID == userID)
                                       .Select(MapToUserDTO)
                                       .FirstOrDefaultAsync();
        }

        public async Task<List<UserDTO>> GetUserDTO(List<int> userIDs)
        {
            return await _context.Users.Where(u => u.ID.In(userIDs))
                                       .Select(MapToUserDTO)
                                       .ToListAsync();
        }

        public async Task<UserDTO?> GetUserDTO(string username)
        {
            return await _context.Users.AsNoTracking()
                                       .Where(u => EF.Functions.Like(u.Username, $"%{username}%"))
                                       .Select(MapToUserDTO)
                                       .FirstOrDefaultAsync();
        }

        public async Task<List<UserDTO>> GetUsersAsync(PostForUserParameters parameters)
        {
            return await _context.Users.AsNoTracking()
                                       .Where(u => (!parameters.UserID.HasValue || u.ID == parameters.UserID)
                                                   && (!parameters.Role.HasValue || u.Role == parameters.Role)
                                                   && (!parameters.CompanyID.HasValue || u.CompanyID == parameters.CompanyID)
                                                   && (!parameters.LeaseID.HasValue || u.LeaseID == parameters.LeaseID)
                                                   && u.Active)
                                       .Select(MapToUserDTO)
                                       .ToListAsync();
        }

        public async Task<List<UserDTO>> GetLeaseUsers(int leaseID)
        {
            return await _context.Users.Where(u => u.LeaseID == leaseID && u.Active)
                                       .Select(MapToUserDTO)
                                       .ToListAsync();
        }

        public async Task UpdateUser(UserDTO userDTO)
        {
            var user = await _context.Users.Where(u => u.ID == userDTO.ID).FirstOrDefaultAsync();

            user.Username = user.Username != userDTO.Username 
                          ? userDTO.Username 
                          : user.Username;
            user.FirstName = user.FirstName != userDTO.FirstName
                          ? userDTO.FirstName
                          : user.FirstName;
            user.LastName = user.LastName != userDTO.LastName
                          ? userDTO.LastName
                          : user.LastName;
            user.Email = user.LastName != userDTO.LastName
                       ? userDTO.LastName
                       : user.LastName;
            user.Phone = user.LastName != userDTO.LastName
                       ? userDTO.LastName
                       : user.LastName;
            user.Active = user.Active != userDTO.Active
                        ? userDTO.Active
                        : user.Active;

            if (_context.ChangeTracker.HasChanges())
            {
                userDTO.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> BatchUpdateUserStatus(int leaseID, bool active)
        {
            var users = _context.Users.Where(u => u.LeaseID == leaseID);
            foreach (var user in users)
            {
                user.Active = active;
                user.UpdatedDate = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task PostUsers(List<User> users)
        {
            await _context.BulkInsertAsync(typeof(Unit), users.ToDataTable());
        }

        private static readonly Expression<Func<User, UserDTO>> MapToUserDTO = static u => new UserDTO
        {
            ID = u.ID,
            CompanyID = u.CompanyID,
            LeaseID = u.LeaseID,
            Lease = new LeaseDTO(u.LeaseID),
            Username = u.Username,
            PasswordHash = u.PasswordHash,
            PasswordSalt = u.PasswordSalt,
            Role = u.Role,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Phone = u.Phone,
            CreatedDate = u.CreatedDate,
            UpdatedDate = u.UpdatedDate,
            Birthdate = u.Birthdate,
            Active = u.Active,
            IsOwner = u.IsOwner
        };

        //private static readonly Expression<Func<User, UserDTO>> MapToUserDTO = static u => new UserDTO
        //{
        //    ID = u.ID,
        //    CompanyID = u.CompanyID,
        //    Company = u.CompanyID.HasValue ? new CompanyDTO
        //    {
        //        ID = u.Company.ID,
        //        Name = u.Company.Name,
        //        Address = new AddressDTO(u.Company.Address, u.Company.City, u.Company.State, u.Company.ZipCode),
        //        Phone = u.Company.Phone,
        //        Email = u.Company.Email,
        //        CreatedDate = u.Company.CreatedDate,
        //        UpdatedDate = u.Company.UpdatedDate,
        //        Active = u.Company.Active
        //    } : null,
        //    LeaseID = u.LeaseID,
        //    Lease = new LeaseDTO(u.LeaseID),
        //    Username = u.Username,
        //    PasswordHash = u.PasswordHash,
        //    PasswordSalt = u.PasswordSalt,
        //    Role = u.Role,
        //    FirstName = u.FirstName,
        //    LastName = u.LastName,
        //    Email = u.Email,
        //    Phone = u.Phone,
        //    CreatedDate = u.CreatedDate,
        //    UpdatedDate = u.UpdatedDate,
        //    Birthdate = u.Birthdate,
        //    Active = u.Active,
        //    IsOwner = u.IsOwner
        //};
    }
}
