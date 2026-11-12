using AspNet.MVC.Models.Categories;

namespace AspNet.MVC.Services.Api
{
    public interface ICategoryApiService
    {
        Task<List<CategoryModel>> GetAllAsync(CancellationToken token = default);

        Task<CategoryModel?> GetByIdAsync(int id,CancellationToken token = default);

        Task<HttpResponseMessage> CreateAsync(CategoryCreateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> UpdateAsync(int id,CategoryUpdateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> DeleteAsync(int id,CancellationToken token = default);
    }
}