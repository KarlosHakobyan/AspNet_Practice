
using AspNet.Data;
using AspNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Repositories
{
    public class ProductDetailsRepository(ApplicationDbContext context)
        : IProductDetailsRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ProductDetails?> GetByProductIdAsync(int productId,CancellationToken token = default)
        {
            return await _context.ProductDetails
                .AsNoTracking().FirstOrDefaultAsync(d => d.ProductId == productId,token);
        }

        
        public async Task AddAsync(ProductDetails details,CancellationToken token = default)
        {
            await _context.ProductDetails.AddAsync(details, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(int productId,ProductDetails details,CancellationToken token = default)
        {
            var existingDetails = await _context.ProductDetails.FirstOrDefaultAsync(d => d.ProductId == productId,token);

            if (existingDetails == null)
                return;

            existingDetails.Description = details.Description;
            existingDetails.Manufacturer = details.Manufacturer;
            existingDetails.WarrantyMonths = details.WarrantyMonths;

            await _context.SaveChangesAsync(token);
        }

        
        public async Task DeleteAsync(int productId,CancellationToken token = default)
        {
            var details = await _context.ProductDetails.FirstOrDefaultAsync(d => d.ProductId == productId,token);

            if (details == null)
                return;

            _context.ProductDetails.Remove(details);

            await _context.SaveChangesAsync(token);
        }
    }
}
