
using AspNet.Entities;
using AspNet.Repositories.Generic;
using AspNet.Repositories.Generics;

namespace AspNet.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
       Task<bool> IsUsedAsync(int tagId,CancellationToken token = default);
    }
}
