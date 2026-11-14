using AspNet.BLL.Common.Results;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductModels;

namespace AspNet.BLL.Services.Products;

public interface IProductService
{
    Task<Result<IEnumerable<Product>>> GetAllAsync(CancellationToken token = default);

    Task<Result<Product>> GetByIdAsync(int id,CancellationToken token = default);

    Task<Result<Product>> GetWithCategoryAsync(int id,CancellationToken token = default);

    Task<Result<Product>> GetFullAsync(int id,CancellationToken token = default);

    Task<Result<Product>> GetWithDetailsAsync(int id,CancellationToken token = default);

    Task<Result<Product>> CreateAsync(ProductAddModel model,CancellationToken token = default);

    Task<Result<Product>> UpdateAsync(int id,ProductUpdateModel model,CancellationToken token = default);

    Task<Result> DeleteAsync(int id,CancellationToken token = default);
}