using AspNet.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Data.Repositories.Generic
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(token);
        }


        public async Task<T?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbSet.FindAsync(new object?[] { id }, token);
        }


        public async Task AddAsync(T entity, CancellationToken token = default)
        {
            await _dbSet.AddAsync(entity, token);
        }

        public Task UpdateAsync(T entity, CancellationToken token = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken token = default)
        {
            var entity = await GetByIdAsync(id, token);

            if (entity == null)
                return;

            _dbSet.Remove(entity);
        }
    }
}
