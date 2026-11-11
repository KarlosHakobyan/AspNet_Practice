using AspNet.MVC.Models.Products;

namespace AspNet.MVC.Services.Api
{
    public interface IProductApiService
    {
        Task<List<ProductModel>> GetAllAsync(CancellationToken token = default);

        Task<ProductModel?> GetByIdAsync(int id,CancellationToken token = default);

        Task<HttpResponseMessage> CreateAsync(ProductCreateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> UpdateAsync(int id,ProductUpdateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> DeleteAsync(int id,CancellationToken token = default);
    }
}