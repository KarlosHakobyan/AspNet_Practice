using AspNet.MVC.Models.Tags;
using System.Net;

namespace AspNet.MVC.Services.Api
{
    public class TagApiService(HttpClient httpClient)
        : ITagApiService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<TagModel>> GetAllAsync(CancellationToken token = default)
        {
            var tags = await _httpClient.GetFromJsonAsync<List<TagModel>>("api/Tags",token);

            return tags ?? new List<TagModel>();
        }

        public async Task<TagModel?> GetByIdAsync(int id,CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"api/Tags/{id}",token);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TagModel>(cancellationToken: token);
        }

        public Task<HttpResponseMessage> CreateAsync(TagCreateModel model,CancellationToken token = default)
        {
            return _httpClient.PostAsJsonAsync("api/Tags",model,token);
        }

        public Task<HttpResponseMessage> UpdateAsync(int id,TagUpdateModel model,CancellationToken token = default)
        {
            return _httpClient.PutAsJsonAsync($"api/Tags/{id}",model,token);
        }

        public Task<HttpResponseMessage> DeleteAsync(int id,CancellationToken token = default)
        {
            return _httpClient.DeleteAsync($"api/Tags/{id}",token);
        }
    }
}