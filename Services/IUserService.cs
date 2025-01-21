using MyBookList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBookList.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
        Task<User> Login(string username, string password);
    }
}