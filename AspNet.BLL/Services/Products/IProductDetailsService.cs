using AspNet.BLL.Common.Results;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductDetailsModels;

namespace AspNet.BLL.Services.Products;

public interface IProductDetailsService
{
    Task<Result<ProductDetails>> GetByIdAsync(int productId, CancellationToken token = default);

    Task<Result<ProductDetails>> CreateAsync(ProductDetailsAddModel model,CancellationToken token = default);

    Task<Result<ProductDetails>> UpdateAsync(int productId,ProductDetailsUpdateModel model,CancellationToken token = default);

    Task<Result> DeleteAsync(int productId,CancellationToken token = default);
}