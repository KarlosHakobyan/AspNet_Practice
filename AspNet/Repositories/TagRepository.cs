
using AspNet.Data;
using AspNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class TagRepository(ApplicationDbContext context) : ITagRepository
    {
        private readonly ApplicationDbContext _context = context;

        
        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken token = default)
        {
            return await _context.Tags
                .AsNoTracking()
                .ToListAsync(token);
        }

       public async Task<Tag?> GetByIdAsync(int tagId,CancellationToken token = default)
        {
            return await _context.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tagId, token);
        }

        
        public async Task AddAsync(Tag tag,CancellationToken token = default)
        {
            await _context.Tags.AddAsync(tag, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(int tagId,Tag tag,CancellationToken token = default)
        {
            var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId, token);

            if (existingTag == null)
                return;

            existingTag.Name = tag.Name;

            await _context.SaveChangesAsync(token);
        }

        
        public async Task DeleteAsync(int tagId,CancellationToken token = default)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId, token);

            if (tag == null)
                return;

            _context.Tags.Remove(tag);

            await _context.SaveChangesAsync(token);
        }

        // CHECK RELATIONSHIP IF U WANT TO DELETE TAG
        public async Task<bool> IsUsedAsync(int tagId,CancellationToken token = default)
        {
            return await _context.ProductTags.AnyAsync(pt => pt.TagId == tagId, token);
        }
    }
}
