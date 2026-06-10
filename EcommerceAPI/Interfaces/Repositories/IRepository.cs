using System.Linq.Expressions;

namespace EcommerceAPI.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T?> GetByIdAsync(object id);
    }
}
