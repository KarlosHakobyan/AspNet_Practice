using AspNet.Entities;
using AspNet.Repositories.Generic;

namespace AspNet.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<bool> IsUsedAsync(int tagId, CancellationToken token = default);
    }
}
