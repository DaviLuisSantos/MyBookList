using MyBookList.Models;
using MyBookList.Data;
using Microsoft.EntityFrameworkCore;
using MyBookList.DTOs.UserBook;
using MyBookList.DTOs.Book;

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
        public async Task<List<UserBook>> GetByUserId(Guid userId)
        {
            return await _context.UserBooks.Where(ub => ub.UserId == userId).Include(x => x.Book).ToListAsync();

        }
        public async Task<List<UserBook>> GetAll()
        {
            return await _context.UserBooks.ToListAsync();
        }
        public async Task<UserBook> Create(UserBookCreateDto userBookDto)
        {
            DateOnly? startDate = userBookDto.StartDate != null ? DateOnly.ParseExact(userBookDto.StartDate, "yyyy-MM-dd", null) : null;
            DateOnly? endDate = userBookDto.FinishDate != null ? DateOnly.ParseExact(userBookDto.FinishDate, "yyyy-MM-dd", null) : null;

            var book = _context.Books.Where(x => x.Isbn == userBookDto.Isbn).FirstOrDefault();

            if (book==null) {
                var newBook = new BookCreateDto
                {
                    Title = userBookDto.Title,
                    Author = userBookDto.Author,
                    Description = userBookDto.Description,
                    Pages = userBookDto.Pages,
                    Genre = userBookDto.Genre,
                    Cover = userBookDto.Cover,
                    Isbn = userBookDto.Isbn,

                };
                book = await _bookService.Create(newBook);
            }

            UserBook userBook = new UserBook
            {
                UserId = userBookDto.UserId,
                BookId = book.BookId,
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
            if (userBook is null)
            {
                return null;
            }
            
            DateOnly startDate;
            if (!string.IsNullOrEmpty(userBookDTO.StartDate))
            {

                if (DateOnly.TryParseExact(userBookDTO.StartDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startDate))
                {
                    userBook.DateStarted = startDate;
                }
                else
                {
                    throw new ArgumentException("Data de inicio inválida");
                }
            }
            else
            {
                userBook.DateStarted = null;
            }
                userBook.Status = userBookDTO.Status;
            

            if (!string.IsNullOrEmpty(userBookDTO.EndDate))
            {
                DateOnly endDate;
                if (DateOnly.TryParseExact(userBookDTO.EndDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDate))
                {
                    userBook.DateFinished = endDate;
                }
                else
                {
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
