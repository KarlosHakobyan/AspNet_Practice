using AspNet.Data;
using AspNet.Entities;
using AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductTag([FromBody] ProductTagsAddModel model,CancellationToken token)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == model.ProductId, token);

            if (!productExists)
                return NotFound("Product does not exist.");

            var tagExists = await _context.Tags.AnyAsync(t => t.Id == model.TagId, token);

            if (!tagExists)
                return NotFound("Tag does not exist.");

            var exists = await _context.ProductTags.AnyAsync(pt =>pt.ProductId == model.ProductId && pt.TagId == model.TagId,token);

            if (exists)
                return Conflict("ProductTag already exists.");

            var productTag = new ProductTag
            {
                ProductId = model.ProductId,
                TagId = model.TagId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProductTags.Add(productTag);

            await _context.SaveChangesAsync(token);

            return Ok(productTag);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var productTags = await _context.ProductTags
                .AsNoTracking()
                .Include(pt => pt.Product)
                .Include(pt => pt.Tag)
                .ToListAsync(token);

            return Ok(productTags);
        }

        [HttpGet("{productId}/{tagId}")]
        public async Task<IActionResult> GetById([FromRoute] int productId,[FromRoute] int tagId,CancellationToken token)
        {
            var productTag = await _context.ProductTags
                .AsNoTracking()
                .Include(pt => pt.Product)
                .Include(pt => pt.Tag)
                .FirstOrDefaultAsync(
                    pt => pt.ProductId == productId && pt.TagId == tagId,token);

            if (productTag == null)
                return NotFound();

            return Ok(productTag);
        }

        [HttpDelete("{productId}/{tagId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId,[FromRoute] int tagId,CancellationToken token)
        {
            var productTag = await _context.ProductTags
                .FirstOrDefaultAsync(
                    pt => pt.ProductId == productId &&
                          pt.TagId == tagId,
                    token);

            if (productTag == null)
                return NotFound("ProductTag relationship not found.");

            _context.ProductTags.Remove(productTag);

            await _context.SaveChangesAsync(token);

            return NoContent();
        }
    }
}
