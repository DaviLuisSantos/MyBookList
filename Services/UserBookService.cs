using MyBookList.Models;
using MyBookList.Data;
using Microsoft.EntityFrameworkCore;
using MyBookList.DTOs.UserBook;

namespace MyBookList.Services
{
    public class UserBookService : IUserBookService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IBookService _bookService;

        public UserBookService(AppDbContext context, IUserService userService, IBookService bookService)
        {
            _context = context;
            _userService = userService;
            _bookService = bookService;
        }

        public async Task<UserBook> GetById(int userBookId)
        {
            return await _context.UserBooks.FindAsync(userBookId);
        }
        public async Task<List<UserBook>> GetByUserId(int userId)
        {
            return await _context.UserBooks.Where(ub => ub.UserId == userId).Include(x => x.Book).ToListAsync();

        }
        public async Task<List<UserBook>> GetByUserUuid(Guid uuid)
        { 
            var user = await _userService.GetUserByUuid(uuid);
            var uBbooks = await _context.UserBooks.Where(ub => ub.UserId == user.Id).ToListAsync();
            List<Book> books = new List<Book> { };
            var tasks = uBbooks.Select(async uBook =>
            {

                var bookUn = await _bookService.GetById(uBook.BookId);
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
            DateOnly? startDate = userBookDto.StartDate != null ? DateOnly.ParseExact(userBookDto.StartDate, "yyyy-MM-dd", null) : null;
            DateOnly? endDate = userBookDto.FinishDate != null ? DateOnly.ParseExact(userBookDto.FinishDate, "yyyy-MM-dd", null) : null;

            UserBook userBook = new UserBook
            {
                UserId = userBookDto.UserId,
                BookId = userBookDto.BookId,
                Status = userBookDto.Status,
                DateStarted = startDate,
                DateFinished = endDate,
            };

            _context.UserBooks.Add(userBook);
            await _context.SaveChangesAsync(); 
            return userBook;
        }
        public async Task<UserBook> UpdateAsync(UserBookUpdateDto userBookDTO)
        {
            var userBook = await _context.UserBooks.FindAsync(userBookDTO.Id);
            if (userBook == null)
            {
                // Lidar com o caso em que o UserBook não é encontrado
                return null;
            }

            DateOnly startDate;
            if (!DateOnly.TryParseExact(userBookDTO.StartDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDate))
            {
                // Lidar com o caso em que a data de início não é válida
                throw new ArgumentException("Data de início inválida");
            }

            userBook.Status = userBookDTO.Status;
            userBook.DateStarted = startDate;

            if (!string.IsNullOrEmpty(userBookDTO.EndDate))
            {
                DateOnly endDate;
                if (DateOnly.TryParseExact(userBookDTO.EndDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDate))
                {
                    userBook.DateFinished = endDate;
                }
                else
                {
                    // Lidar com o caso em que a data de término não é válida
                    throw new ArgumentException("Data de término inválida");
                }
            }
            else
            {
                userBook.DateFinished = null;
            }

            _context.UserBooks.Update(userBook);
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
