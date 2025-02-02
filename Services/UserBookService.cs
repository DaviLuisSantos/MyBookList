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
        public async Task<List<UserBook>> GetByUserUuid(Guid uuid)
        {
            UserService userService = new UserService(_context);
            BookService bookService = new BookService(_context);
            var user = await userService.GetUserByUuid(uuid);
            var uBbooks = await _context.UserBooks.Where(ub => ub.UserId == user.Id).ToListAsync();
            List<Book> books = new List<Book> { };
            var tasks = uBbooks.Select(async uBook =>
            {
                var bookUn = await bookService.GetById(uBook.BookId);
                if (bookUn != null)
                {
                    uBook.Book = bookUn;
                    books.Add(bookUn);
                }

            });

            await Task.WhenAll(tasks);

            return uBbooks;
        }
        public async Task<List<UserBook>> GetAll()
        {
            return await _context.UserBooks.ToListAsync();
        }
        public async Task<UserBook> Create(UserBookCreateDto userBookDto)
        {
            DateTime startDate = DateTime.ParseExact(userBookDto.StartDate, "yyyy-MM-dd", null);
            DateTime? endDate = userBookDto.FinishDate != null ? DateTime.ParseExact(userBookDto.FinishDate, "yyyy-MM-dd", null) : null;

            UserService userService = new UserService(_context);
            BookService bookService = new BookService(_context);
            UserBook userBook = new UserBook
            {
                Status = userBookDto.Status,
                DateStarted = startDate,
                DateFinished = endDate,
            };

            User? user = await userService.GetUserByUuid(userBookDto.UserUuid.Value);
            Book? book = await bookService.GetById(userBookDto.BookId);

            userBook.UserId = user.Id;
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
