using AspNet.MVC.Models.ProductDetails;

namespace AspNet.MVC.Services.Api
{
    public interface IProductDetailsApiService
    {
        Task<ProductDetailsModel?> GetByIdAsync(int productId,CancellationToken token = default);

        Task<HttpResponseMessage> CreateAsync(ProductDetailsCreateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> UpdateAsync(int productId,ProductDetailsUpdateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> DeleteAsync(int productId,CancellationToken token = default);
    }
}