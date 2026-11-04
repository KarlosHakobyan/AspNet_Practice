using AspNet.Entities;
using AspNet.Repositories.Generic;

namespace AspNet.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default);
    }
}