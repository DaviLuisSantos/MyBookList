using Microsoft.EntityFrameworkCore;
using MyBookList.Data;
using MyBookList.Models;
using MyBookList.DTOs.Book;

namespace MyBookList.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;
        public BookService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Book> GetById(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }
        public async Task<List<Book>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<Book> Create(BookCreateDto bookDto)
        {

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Description = bookDto.Description,
                Pages = bookDto.Pages,
                Genre = bookDto.Genre,
                Cover = bookDto.Cover,
                Isbn =  bookDto.Isbn,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<Book> Update(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<bool> Delete(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return false;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
