
using AspNet.Data;
using AspNet.Entities;
using AspNet.Repositories.Generic;
using AspNet.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class TagRepository(ApplicationDbContext context) : GenericRepository<Tag>(context), ITagRepository
    {
        // CHECK RELATIONSHIP IF U WANT TO DELETE TAG
        public async Task<bool> IsUsedAsync(int tagId,CancellationToken token = default)
        {
            return await _context.ProductTags.AnyAsync(pt => pt.TagId == tagId, token);
        }
    }
}
