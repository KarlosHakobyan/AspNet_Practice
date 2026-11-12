using AspNet.MVC.Models.ProductDetails;
using System.Net;
using System.Net.Http.Json;

namespace AspNet.MVC.Services.Api
{
    public class ProductDetailsApiService(HttpClient httpClient)
        : IProductDetailsApiService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ProductDetailsModel?> GetByIdAsync(int productId,CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"api/ProductDetails/{productId}", token);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductDetailsModel>(cancellationToken: token);
        }

        public Task<HttpResponseMessage> CreateAsync(ProductDetailsCreateModel model,CancellationToken token = default)
        {
            return _httpClient.PostAsJsonAsync("api/ProductDetails", model, token);
        }

        public Task<HttpResponseMessage> UpdateAsync(int productId,ProductDetailsUpdateModel model,CancellationToken token = default)
        {
            return _httpClient.PutAsJsonAsync($"api/ProductDetails/{productId}", model, token);
        }

        public Task<HttpResponseMessage> DeleteAsync(int productId,CancellationToken token = default)
        {
            return _httpClient.DeleteAsync($"api/ProductDetails/{productId}", token);
        }
    }
}