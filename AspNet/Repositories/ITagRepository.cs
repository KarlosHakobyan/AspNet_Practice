
using AspNet.Entities;

namespace AspNet.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(CancellationToken token = default);

        Task<Tag?> GetByIdAsync(int tagId,CancellationToken token = default);

        Task AddAsync(Tag tag,CancellationToken token = default);

        Task UpdateAsync(int tagId,Tag tag,CancellationToken token = default);

        Task DeleteAsync(int tagId,CancellationToken token = default);

        Task<bool> IsUsedAsync(int tagId,CancellationToken token = default);
    }
}
