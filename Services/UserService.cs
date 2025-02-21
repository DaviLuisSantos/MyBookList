using Microsoft.EntityFrameworkCore;
using MyBookList.Data;
using MyBookList.Models;
using MyBookList.Authentication;
using MyBookList.DTOs.User;

namespace MyBookList.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _email;

        public UserService(AppDbContext context, IEmailSender email)
        {
            _context = context;
            _email = email;
        }

        public async Task<User> GetById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Create(UserCreateDto userNv)
        {
            var userExist = _context.Users.FirstOrDefaultAsync(u => u.Email == userNv.Email);
            if (userExist != null&& userExist.Result!=null) return new User();
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = userNv.Username,
                Email = userNv.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userNv.Password),
                Activated = userNv.Active ?? false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            string id = user.Id.ToString();
            await _email.SendAccountActivationEmailAsync(userNv.Email, id);
            return user;
        }

        public async Task<bool> Active(string email, Guid userActive)
        {
            var user = await GetById(userActive);
            if (user!=null)
            {
                user.Activated = true;
                await Update(user);
                return true;
            }
            return false;
        }

        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Delete(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LoginReturn> Login(LoginDto login)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.Activated==true);
            if (user == null)
                return new LoginReturn();
            if(!VerifyPasswordHash(login.Password, user.PasswordHash))
                return new LoginReturn
                {
                    token="0"
                };

            string token = AuthService.GenerateToken(user);
            return new LoginReturn
            {
                token = token,
                Id = user.Id,
                Username = user.Username
            };

        }

        public async Task<LoginReturn> GoogleLogin(LoginDto login)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null)
            {
                var createUser = new UserCreateDto
                {
                    Username = login.Username,
                    Password = login.Password,
                    Email = login.Email,
                    Active = true
                };
                user = await Create(createUser);
            }

            return await Login(login);

        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Implement your password hash verification logic here
            // For example, if you are using a simple hash:
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}