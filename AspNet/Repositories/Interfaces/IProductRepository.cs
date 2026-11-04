using AspNet.Entities;
using AspNet.Repositories.Generics;

namespace AspNet.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetWithCategoryAsync(int productId, CancellationToken token = default);

        Task<Product?> GetWithDetailsAsync(int productId, CancellationToken token = default);

        Task<Product?> GetFullAsync(int productId, CancellationToken token = default);
    }
}
