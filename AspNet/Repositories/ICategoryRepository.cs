using AspNet.Entities;

namespace AspNet.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category, CancellationToken token = default);
        Task DeleteAsync(int categoryId, CancellationToken token = default);
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken token = default);
        Task<Category?> GetByIdAsync(int categoryId, CancellationToken token = default);
        Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default);
        Task UpdateAsync(int categoryId, Category category, CancellationToken token);
    }
}