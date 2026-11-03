
using AspNet.Entities;

namespace AspNet.Repositories
{
    public interface IProductDetailsRepository
    {
        Task<ProductDetails?> GetByProductIdAsync(int productId,CancellationToken token = default);

        Task AddAsync(ProductDetails details,CancellationToken token = default);

        Task UpdateAsync(int productId,ProductDetails details,CancellationToken token = default);

        Task DeleteAsync(int productId,CancellationToken token = default);
    }
}
