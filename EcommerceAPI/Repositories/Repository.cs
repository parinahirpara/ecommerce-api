using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcommerceAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbContextSet;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextSet = dbContext.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbContextSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task<T?> GetAsync(Guid id)
        {
            return await _dbContextSet.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContextSet.ToListAsync();
        }
        public virtual async Task Delete(T entity)
        {
            _dbContextSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task Update(T entity)
        {
            _dbContextSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await _dbContextSet.FindAsync(id);
        }
    }
}
