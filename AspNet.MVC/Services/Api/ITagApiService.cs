using AspNet.MVC.Models.Tags;

namespace AspNet.MVC.Services.Api
{
    public interface ITagApiService
    {
        Task<List<TagModel>> GetAllAsync(CancellationToken token = default);

        Task<TagModel?> GetByIdAsync(int id,CancellationToken token = default);

        Task<HttpResponseMessage> CreateAsync(TagCreateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> UpdateAsync(int id,TagUpdateModel model,CancellationToken token = default);

        Task<HttpResponseMessage> DeleteAsync(int id,CancellationToken token = default);
    }
}