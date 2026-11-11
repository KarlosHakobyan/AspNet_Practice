using AspNet.Domain.Entities;
using AspNet.Domain.Models.TagModels;

namespace AspNet.BLL.Services.Products
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync(CancellationToken token = default);

        Task<Tag?> GetByIdAsync(int id, CancellationToken token = default);

        Task<Tag> CreateAsync(TagAddModel model, CancellationToken token = default);

        Task<Tag?> UpdateAsync(int id, TagUpdateModel model, CancellationToken token = default);

        Task<bool> DeleteAsync(int id, CancellationToken token = default);
    }
}
