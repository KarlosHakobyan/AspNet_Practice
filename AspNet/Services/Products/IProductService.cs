using AspNet.Entities;
using AspNet.Models.ProductModels;


namespace AspNet.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

        Task<Product?> GetByIdAsync(int id, CancellationToken token = default);

        Task CreateAsync(ProductAddModel model, CancellationToken token = default);

        Task<Product?> UpdateAsync(int id, ProductUpdateModel model, CancellationToken token = default);

        Task<bool> DeleteAsync(int id, CancellationToken token = default);

        Task<Product?> GetWithCategoryAsync(int id,CancellationToken token = default);

        Task<Product?> GetFullAsync(int id,CancellationToken token = default);

        Task<Product?> GetWithDetailsAsync(int id, CancellationToken token);
    }
}