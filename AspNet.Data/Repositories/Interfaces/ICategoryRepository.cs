using AspNet.Data.Repositories.Generic;
using AspNet.Domain.Entities;

namespace AspNet.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default);
    }
}