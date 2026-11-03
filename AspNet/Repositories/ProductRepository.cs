
using AspNet.Data;
using AspNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class ProductRepository(ApplicationDbContext context) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
        {
            var products = await _context.Products.AsNoTracking().ToListAsync(token);

            return products;
        }

        public async Task<Product?> GetByIdAsync(int productId,CancellationToken token = default)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId, token);

            return product;
        }

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


        public async Task AddAsync(Product product,CancellationToken token = default)
        {
            await _context.Products.AddAsync(product, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Product product,CancellationToken token = default)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(int productId,CancellationToken token = default)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId, token);

            if (product == null)
                return;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(token);
        }
    }
}
