using MyBookList.Models;
using MyBookList.Data;
using Microsoft.EntityFrameworkCore;

namespace MyBookList.Services
{
    public class UserBookService : IUserBookService
    {
        private readonly AppDbContext _context;
        public UserBookService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserBook> GetUserBookById(int userBookId)
        {
            return await _context.UserBooks.FindAsync(userBookId);
        }
        public async Task<IEnumerable<UserBook>> GetUserBooksByUserId(int userId)
        {
            return await _context.UserBooks.Where(ub => ub.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<UserBook>> GetAllUserBooks()
        {
            return await _context.UserBooks.ToListAsync();
        }
        public async Task<UserBook> CreateUserBook(UserBook userBook)
        {
            _context.UserBooks.Add(userBook);
            await _context.SaveChangesAsync();
            return userBook;
        }
        public async Task<UserBook> UpdateUserBook(UserBook userBook)
        {
            _context.Entry(userBook).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userBook;
        }
        public async Task<bool> DeleteUserBook(int userBookId)
        {
            var userBook = await _context.UserBooks.FindAsync(userBookId);
            if (userBook == null)
                return false;
            _context.UserBooks.Remove(userBook);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
