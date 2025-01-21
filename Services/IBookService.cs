using MyBookList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBookList.Services
{
    public interface IBookService
    {
        Task<Book> GetBookById(int bookId);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> CreateBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<bool> DeleteBook(int bookId);
    }
}
