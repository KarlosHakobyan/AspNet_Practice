using AspNet.Data;
using AspNet.Data.Repositories.Generic;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Data.Repositories
{
    public class CategoryRepository(ApplicationDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        public async Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId, token);
        }
    }
}
