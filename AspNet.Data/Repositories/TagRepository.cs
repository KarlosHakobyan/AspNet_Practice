using AspNet.Data;
using AspNet.Data.Repositories.Generic;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Data.Repositories
{
    public class TagRepository(ApplicationDbContext context) : GenericRepository<Tag>(context), ITagRepository
    {
        // CHECK RELATIONSHIP IF U WANT TO DELETE TAG
        public async Task<bool> IsUsedAsync(int tagId, CancellationToken token = default)
        {
            return await _context.ProductTags.AnyAsync(pt => pt.TagId == tagId, token);
        }
    }
}
