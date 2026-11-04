using AspNet.Entities;

namespace AspNet.Repositories.Interfaces
{
    public interface IProductTagRepository
    {
        Task<IEnumerable<ProductTag>> GetAllAsync(CancellationToken token = default);

        Task<ProductTag?> GetByIdsAsync(int productId, int tagId, CancellationToken token = default);

        Task<IEnumerable<ProductTag>> GetByProductIdAsync(int productId, CancellationToken token = default);

        Task AddAsync(ProductTag productTag, CancellationToken token = default);

        Task DeleteAsync(int productId, int tagId, CancellationToken token = default);

        Task<bool> ExistsAsync(int productId, int tagId, CancellationToken token = default);
    }
}