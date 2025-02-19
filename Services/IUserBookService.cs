using MyBookList.Models;
using MyBookList.DTOs.UserBook;

namespace MyBookList.Services
{
    public interface IUserBookService
    {
        Task<UserBook> GetById(int userBookId);
        Task<List<UserBook>> GetAll();
        Task<List<UserBook>> GetByUserId(Guid userId);
        Task<UserBook> Create(UserBookCreateDto userBook);
        Task<UserBook> UpdateAsync(UserBookUpdateDto userBook);
        Task<bool> Delete(int userBookId);
    }
}
