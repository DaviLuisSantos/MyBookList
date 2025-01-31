using MyBookList.Models;
using MyBookList.DTOs.User;

namespace MyBookList.Services
{
    public interface IUserService: ICrudService<User, UserCreateDto>
    {
        Task<User> GetById(int userId);
        Task<User> GetByUsername(string username);
        Task<List<User>> GetAll();
        Task<User> Create(UserCreateDto userNv);
        Task<User> Update(User user);
        Task<bool> Delete(int userId);
        Task<string> Login(LoginDto login);
    }
}