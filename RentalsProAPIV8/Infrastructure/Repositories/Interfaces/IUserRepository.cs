using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Client.Parameters;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<UserDTO?> GetUserDTO(int userID);
        Task<List<UserDTO>> GetUserDTO(List<int> userIDs);
        Task<UserDTO?> GetUserDTO(string username);
        Task<List<UserDTO>> GetUsersAsync(PostForUserParameters parameters);
        Task<List<UserDTO>> GetLeaseUsers(int leaseID);
        Task UpdateUser(UserDTO userDTO);
        Task<int> BatchUpdateUserStatus(int leaseID, bool active);
        Task PostUsers(List<User> users);
    }
}
