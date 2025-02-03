using MyBookList.Models;
using MyBookList.DTOs.UserBook;

namespace MyBookList.Services
{
    public interface IUserBookService
    {
        Task<UserBook> GetById(int userBookId);
        Task<List<UserBook>> GetAll();
        Task<List<UserBook>> GetByUserId(int userId);
        Task<List<UserBook>> GetByUserUuid(Guid uuid);
        Task<UserBook> Create(UserBookCreateDto userBook);
        Task<UserBook> UpdateAsync(UserBookUpdateDto userBook);
        Task<bool> Delete(int userBookId);
    }
}
