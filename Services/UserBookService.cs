using MyBookList.Models;
using MyBookList.Data;
using Microsoft.EntityFrameworkCore;
using MyBookList.Services;

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
        public async Task<UserBook> CreateUserBook(UserBook userBook,Guid uuid)
        {
            UserService userService = new UserService(_context);
            BookService bookService = new BookService(_context);
            var user = await userService.GetUserByUuid(uuid);
            var book = await bookService.GetBookById(userBook.BookId);

            userBook.UserId = user.UserId;
            userBook.BookId = book.BookId;

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
