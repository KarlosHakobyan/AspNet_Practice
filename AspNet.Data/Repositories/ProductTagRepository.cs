using AspNet.Data;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Data.Repositories
{
    public class ProductTagRepository(ApplicationDbContext context) : IProductTagRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<ProductTag>> GetAllAsync(CancellationToken token = default)
        {
            return await _context.ProductTags
                .AsNoTracking()
                .Include(pt => pt.Product)
                .Include(pt => pt.Tag)
                .ToListAsync(token);
        }

        public async Task<ProductTag?> GetByIdsAsync(int productId, int tagId, CancellationToken token = default)
        {
            return await _context.ProductTags
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    pt => pt.ProductId == productId &&
                          pt.TagId == tagId,
                    token);
        }

        public async Task<IEnumerable<ProductTag>> GetByProductIdAsync(int productId, CancellationToken token = default)
        {
            return await _context.ProductTags
                .AsNoTracking()
                .Include(pt => pt.Tag)
                .Where(pt => pt.ProductId == productId)
                .ToListAsync(token);
        }


        public async Task AddAsync(ProductTag productTag, CancellationToken token = default)
        {
            await _context.ProductTags.AddAsync(productTag, token);
        }


        public async Task DeleteAsync(int productId, int tagId, CancellationToken token = default)
        {
            var productTag = await _context.ProductTags.FirstOrDefaultAsync(pt => pt.ProductId == productId && pt.TagId == tagId, token);

            if (productTag == null)
                return;

            _context.ProductTags.Remove(productTag);
        }


        public async Task<bool> ExistsAsync(int productId, int tagId, CancellationToken token = default)
        {
            return await _context.ProductTags.AnyAsync(pt => pt.ProductId == productId && pt.TagId == tagId, token);
        }
    }
}
