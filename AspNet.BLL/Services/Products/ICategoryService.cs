using AspNet.Domain.Entities;
using AspNet.Domain.Models.CategoryModels;

namespace AspNet.Api.Services.Products
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken token = default);

        Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default);

        Task<Category?> GetByIdAsync(int id, CancellationToken token = default);

        Task CreateAsync(CategoryAddModel model, CancellationToken token = default);

        Task<Category?> UpdateAsync(int id, CategoryUpdateModel model, CancellationToken token = default);

        Task<bool> DeleteAsync(int id, CancellationToken token = default);
    }
}
