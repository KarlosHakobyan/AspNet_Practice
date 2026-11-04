
using AspNet.Data;
using AspNet.Entities;
using AspNet.Repositories.Generic;
using AspNet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : GenericRepository<Product>(context), IProductRepository
    {

        public async Task<Product?> GetWithCategoryAsync(int productId, CancellationToken token = default)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId, token);

            return product;
        }

        public async Task<Product?> GetWithDetailsAsync(int productId, CancellationToken token = default)
        {
            var product = await _context.Products
                .Include(p => p.Details)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId, token);

            return product;
        }

        public async Task<Product?> GetFullAsync(int productId, CancellationToken token = default)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Details)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId, token);

            return product;
        }       
    }
}
