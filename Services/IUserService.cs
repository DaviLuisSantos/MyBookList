using MyBookList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBookList.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task<List<User>> GetAll();
        Task<bool> CreateUser(string username,string password, string email);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
        Task<string> Login(string username, string password);
    }
}