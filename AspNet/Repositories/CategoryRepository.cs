
using AspNet.Data;
using AspNet.Entities;
using AspNet.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class CategoryRepository(ApplicationDbContext context) : GenericRepository<Category>(context) , ICategoryRepository
    {
        public async Task<Category?> GetWithProductsAsync(int categoryId,CancellationToken token = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId, token);
        }                
    }
}
