using AspNet.Data;
using AspNet.Entities;
using AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddModel model, CancellationToken token)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == model.CategoryId, token);

            if (!categoryExists)
                return BadRequest("Category does not exist.");

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(token);

            return Ok(product);
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken token)
        {
            var products = await _context.Products.ToListAsync(token);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id, CancellationToken token)
        {
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == id, token);


            if (product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpGet("{id}/category")]
        public async Task<IActionResult> GetProductWithCategory([FromRoute] int id,CancellationToken token)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id, token);

            if (product == null)
                return NotFound();

            var result = new ProductWithCategoryModel
            {
                Id = product.Id,
                Name = product.Name,

                Category = product.Category == null
                    ? null
                    : new CategoryModel
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    }
            };

            return Ok(result);
        }

        //Miangamic Entitynerov get anel 
        /*        [HttpGet("{id}/full")]
                public async Task<IActionResult> GetFullProductInfo(
                 [FromRoute] int id,
                 CancellationToken token)
                {
                    var product = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Details)
                        .Include(p => p.ProductTags)
                            .ThenInclude(pt => pt.Tag)
                        .FirstOrDefaultAsync(p => p.Id == id, token);

                    if (product == null)
                        return NotFound();

                    return Ok(product);
                }*/

        //Modelnerov get anel

        [HttpGet("{id}/full")]
        public async Task<IActionResult> GetFullProductInfo([FromRoute] int id,CancellationToken token)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Details)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id, token);

            if (product == null)
                return NotFound();

            var result = new ProductFullModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,

                Category = product.Category == null
                    ? null
                    : new CategoryModel
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    },

                Details = product.Details == null
                    ? null
                    : new ProductDetailsModel
                    {
                        Description = product.Details.Description,
                        Manufacturer = product.Details.Manufacturer,
                        WarrantyMonths = product.Details.WarrantyMonths
                    },

                Tags = product.ProductTags.Select(pt => new TagModel
                {
                    Id = pt.Tag.Id,
                    Name = pt.Tag.Name
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] int id,CancellationToken token)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.Id == id, token);

            if (product == null)
                return NotFound();

            var result = new ProductWithDetailsModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,

                Details = product.Details == null ? null : new ProductDetailsModel
                {
                    Description = product.Details.Description,
                    Manufacturer = product.Details.Manufacturer,
                    WarrantyMonths = product.Details.WarrantyMonths
                }
            };

            return Ok(result);
        }



        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProductById([FromRoute] int id, [FromBody] ProductUpdateModel model, CancellationToken token)
        {
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == id, token);
            if (product == null)
                return NotFound();

            product.Price = model.Price;
            product.Name = model.Name;

            await _context.SaveChangesAsync(token);
            return Ok($"Product updated\n {product}");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductById([FromRoute] int id, CancellationToken token)
        {
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == id, token);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(token);
            return Ok("Product deleted");

        }


    }
}
