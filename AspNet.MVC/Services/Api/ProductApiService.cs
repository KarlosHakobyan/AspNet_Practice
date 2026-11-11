using AspNet.MVC.Models.Products;
using System.Net;
using System.Net.Http.Json;

namespace AspNet.MVC.Services.Api
{
    public class ProductApiService(HttpClient httpClient)
        : IProductApiService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<ProductModel>> GetAllAsync(CancellationToken token = default)
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductModel>>("api/Product",token);

            return products ?? new List<ProductModel>();
        }

        public async Task<ProductModel?> GetByIdAsync(int id,CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"api/Product/{id}",token);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductModel>(cancellationToken: token);
        }

        public Task<HttpResponseMessage> CreateAsync(ProductCreateModel model,CancellationToken token = default)
        {
            return _httpClient.PostAsJsonAsync("api/Product",model,token);
        }

        public Task<HttpResponseMessage> UpdateAsync(int id,ProductUpdateModel model,CancellationToken token = default)
        {
            return _httpClient.PutAsJsonAsync($"api/Product/{id}",model,token);
        }

        public Task<HttpResponseMessage> DeleteAsync(int id,CancellationToken token = default)
        {
            return _httpClient.DeleteAsync($"api/Product/{id}",token);
        }
    }
}