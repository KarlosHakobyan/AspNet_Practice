using AspNet.MVC.Models.Categories;
using System.Net;
using System.Net.Http.Json;

namespace AspNet.MVC.Services.Api
{
    public class CategoryApiService(HttpClient httpClient)
        : ICategoryApiService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<CategoryModel>> GetAllAsync(CancellationToken token = default)
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryModel>>("api/Category",token);

            return categories ?? new List<CategoryModel>();
        }

        public async Task<CategoryModel?> GetByIdAsync(int id,CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"api/Category/{id}",
                token);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryModel>(cancellationToken: token);
        }

        public Task<HttpResponseMessage> CreateAsync(CategoryCreateModel model,CancellationToken token = default)
        {
            return _httpClient.PostAsJsonAsync("api/Category",model,token);
        }

        public Task<HttpResponseMessage> UpdateAsync(int id,CategoryUpdateModel model,CancellationToken token = default)
        {
            return _httpClient.PutAsJsonAsync($"api/Category/{id}",model,token);
        }

        public Task<HttpResponseMessage> DeleteAsync(int id,CancellationToken token = default)
        {
            return _httpClient.DeleteAsync($"api/Category/{id}",token);
        }
    }
}