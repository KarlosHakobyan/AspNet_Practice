using AspNet.Entities;
using AspNet.Repositories.Generics;

namespace AspNet.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default);
    }
}