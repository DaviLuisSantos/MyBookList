using Microsoft.EntityFrameworkCore;
using MyBookList.Data;
using MyBookList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBookList.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;
        public BookService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Book> GetBookById(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<Book> CreateBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<Book> UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<bool> DeleteBook(int bookId)
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
