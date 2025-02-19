using MyBookList.Models;
using MyBookList.DTOs.User;

namespace MyBookList.Services
{
    public interface IUserService
    {
        Task<User> GetById(Guid userId);
        Task<List<User>> GetAll();
        Task<User> Create(UserCreateDto userNv);
        Task<User> Update(User user);
        Task<bool> Delete(int userId);
        Task<LoginReturn> Login(LoginDto login);
        Task<LoginReturn> GoogleLogin(LoginDto login);
    }
}