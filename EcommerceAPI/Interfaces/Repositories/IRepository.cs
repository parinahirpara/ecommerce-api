using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcommerceAPI.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> GetAllQueryable();
        Task<T?> GetByIdAsync(object id);
        Task<int> SaveChangesAsync();
    }
}
