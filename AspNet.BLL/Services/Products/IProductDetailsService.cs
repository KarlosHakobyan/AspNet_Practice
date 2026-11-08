using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductDetailsModels;

namespace AspNet.Api.Services.Products
{
    public interface IProductDetailsService
    {
        Task<ProductDetails?> GetByIdAsync(int productId, CancellationToken token = default);

        Task<ProductDetails?> CreateAsync(ProductDetailsAddModel model, CancellationToken token = default);

        Task<ProductDetails?> UpdateAsync(int productId, ProductDetailsUpdateModel model, CancellationToken token = default);

        Task<bool> DeleteAsync(int productId, CancellationToken token = default);
    }
}