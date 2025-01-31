using Microsoft.EntityFrameworkCore;
using MyBookList.Data;
using MyBookList.Models;
using MyBookList.Authentication;

namespace MyBookList.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> CreateUser(string username,string password,string email)
        {
            User user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> Login(string username, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;
            if(!VerifyPasswordHash(password, user.PasswordHash))
                return "0";

            return AuthService.GenerateToken(user);

        }

        public async Task<User> GetUserByUuid(System.Guid uuid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserUuid == uuid);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Implement your password hash verification logic here
            // For example, if you are using a simple hash:
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}