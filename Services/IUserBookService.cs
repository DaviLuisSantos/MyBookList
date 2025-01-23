using MyBookList.Models;   

namespace MyBookList.Services
{
    public interface IUserBookService
    {
        Task<UserBook> GetUserBookById(int userBookId);
        Task<IEnumerable<UserBook>> GetAllUserBooks();
        Task<IEnumerable<UserBook>> GetUserBooksByUserId(int userId);
        Task<UserBook> CreateUserBook(UserBook userBook,Guid uuid);
        Task<UserBook> UpdateUserBook(UserBook userBook);
        Task<bool> DeleteUserBook(int userBookId);
    }
}
