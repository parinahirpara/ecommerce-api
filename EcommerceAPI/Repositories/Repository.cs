using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
        }
        public virtual async Task<T?> GetAsync(Guid id)
        {
            return await _dbContextSet.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContextSet.ToListAsync();
        }
        public virtual void Delete(T entity)
        {
            _dbContextSet.Remove(entity);
        }
        public virtual void Update(T entity)
        {
            _dbContextSet.Update(entity);
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await _dbContextSet.FindAsync(id);
        }
        public IQueryable<T> GetAllQueryable()
        {
            return _dbContextSet.AsNoTracking();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
