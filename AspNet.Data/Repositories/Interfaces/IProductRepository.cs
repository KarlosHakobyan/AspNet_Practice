using AspNet.Data.Repositories.Generic;
using AspNet.Domain.Entities;

namespace AspNet.Data.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetWithCategoryAsync(int productId, CancellationToken token = default);

        Task<Product?> GetWithDetailsAsync(int productId, CancellationToken token = default);

        Task<Product?> GetFullAsync(int productId, CancellationToken token = default);
    }
}
