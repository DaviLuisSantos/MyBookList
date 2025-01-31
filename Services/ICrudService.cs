using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBookList.Services
{
    public interface ICrudService<T, TCreateDto>
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<T> Create(TCreateDto entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
    }
}
