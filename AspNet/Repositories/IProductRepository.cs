using AspNet.Entities;

namespace AspNet.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

        Task<Product?> GetByIdAsync(int productId,CancellationToken token = default);

        Task AddAsync(Product product,CancellationToken token = default);

        Task UpdateAsync(Product product,CancellationToken token = default);

        Task DeleteAsync(int productId,CancellationToken token = default);

        Task<Product?> GetWithCategoryAsync(int productId,CancellationToken token = default);

        Task<Product?> GetWithDetailsAsync(int productId,CancellationToken token = default);

        Task<Product?> GetFullAsync(int productId,CancellationToken token = default);
    }
}
