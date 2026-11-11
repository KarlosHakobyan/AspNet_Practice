
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductModels;

namespace AspNet.BLL.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

        Task<Product?> GetByIdAsync(int id, CancellationToken token = default);

        Task<Product?> GetWithCategoryAsync(int id, CancellationToken token = default);

        Task<Product?> GetFullAsync(int id, CancellationToken token = default);

        Task<Product?> GetWithDetailsAsync(int id, CancellationToken token = default);

        Task<bool> CreateAsync(ProductAddModel model, CancellationToken token = default);

        Task<bool> UpdateAsync(int id, ProductUpdateModel model, CancellationToken token = default);

        Task<bool> DeleteAsync(int id, CancellationToken token = default);
    }
}
