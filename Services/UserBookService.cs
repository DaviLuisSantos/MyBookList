using MyBookList.Models;
using MyBookList.Data;
using Microsoft.EntityFrameworkCore;
using MyBookList.DTOs.UserBook;

namespace MyBookList.Services
{
    public class UserBookService : IUserBookService
    {
        private readonly AppDbContext _context;
        public UserBookService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserBook> GetById(int userBookId)
        {
            return await _context.UserBooks.FindAsync(userBookId);
        }
        public async Task<List<UserBook>> GetByUserId(int userId)
        {
            return await _context.UserBooks.Where(ub => ub.UserId == userId).ToListAsync();
        }
        public async Task<List<UserBook>> GetAll()
        {
            return await _context.UserBooks.ToListAsync();
        }
        public async Task<UserBook> Create(UserBookCreateDto userBookDto)
        {
            DateTime startDate = DateTime.ParseExact(userBookDto.StartDate, "yyyy-MM-dd", null);
            DateTime endDate = DateTime.ParseExact(userBookDto.FinishDate, "yyyy-MM-dd", null);
            UserService userService = new UserService(_context);
            BookService bookService = new BookService(_context);
            UserBook userBook = new UserBook
            {
                Status = userBookDto.Status,
                DateStarted = startDate,
                DateFinished = endDate,
            };

            var user = await userService.GetUserByUuid(userBookDto.UserUuid.Value);
            var book = await bookService.GetById(userBook.BookId);

            userBook.UserId = user.UserId;
            userBook.BookId = book.BookId;

            _context.UserBooks.Add(userBook);
            await _context.SaveChangesAsync();
            return userBook;
        }
        public async Task<UserBook> Update(UserBook userBook)
        {
            _context.Entry(userBook).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userBook;
        }
        public async Task<bool> Delete(int userBookId)
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
