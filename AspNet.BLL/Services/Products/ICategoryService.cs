using AspNet.BLL.Common.Results;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.CategoryModels;

namespace AspNet.BLL.Services.Products;

public interface ICategoryService
{
    Task<Result<IEnumerable<Category>>> GetAllAsync(CancellationToken token = default);

    Task<Result<Category>> GetWithProductsAsync(int categoryId,CancellationToken token = default);

    Task<Result<Category>> GetByIdAsync(int id,CancellationToken token = default);

    Task<Result<Category>> CreateAsync(CategoryAddModel model,CancellationToken token = default);

    Task<Result<Category>> UpdateAsync(int id,CategoryUpdateModel model,CancellationToken token = default);

    Task<Result> DeleteAsync(int id,CancellationToken token = default);
}