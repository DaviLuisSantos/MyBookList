using MyBookList.Models;
using MyBookList.DTOs.Book;

namespace MyBookList.Services
{
    public interface IBookService : ICrudService<Book, BookCreateDto>
    {
        Task<Book> GetById(int bookId);
        Task<List<Book>> GetAll();
        Task<Book> Create(BookCreateDto book);
        Task<Book> Update(Book book);
        Task<bool> Delete(int bookId);
    }
}
