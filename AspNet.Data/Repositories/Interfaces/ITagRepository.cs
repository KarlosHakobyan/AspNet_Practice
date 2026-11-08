using AspNet.Data.Repositories.Generic;
using AspNet.Domain.Entities;

namespace AspNet.Data.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<bool> IsUsedAsync(int tagId, CancellationToken token = default);
    }
}
