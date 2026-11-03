
using AspNet.Data;
using AspNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class CategoryRepository(ApplicationDbContext context)
        : ICategoryRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Category category,CancellationToken token = default)
        {
            await _context.Categories.AddAsync(category, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(
            CancellationToken token = default)
        {
            return await _context.Categories.AsNoTracking().ToListAsync(token);
        }

        public async Task<Category?> GetByIdAsync(int categoryId,CancellationToken token = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId, token);
        }

        public async Task<Category?> GetWithProductsAsync(int categoryId,CancellationToken token = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId, token);
        }

        public async Task UpdateAsync(int categoryId,Category category,CancellationToken token = default)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, token);

            if (existingCategory == null)
                return;

            existingCategory.Name = category.Name;

            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(int categoryId,CancellationToken token = default)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, token);

            if (category == null)
                return;

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync(token);
        }
    }
}
