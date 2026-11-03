using AspNet.Data;
using AspNet.Entities;
using AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]

        public async Task<IActionResult> AddCategory([FromBody] CategoryAddModel model, CancellationToken token)
        {
            var category = new Category
            {
                Name = model.Name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(token);
            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken token)
        {
            var categories = await _context.Categories.ToListAsync(token);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken token)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id, token);


            if (category == null)
                return NotFound();

            return Ok(category);
        }
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts([FromRoute] int id,CancellationToken token)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id, token);

            if (category == null)
                return NotFound();

            var result = new CategoryWithProductsModel
            {
                Id = category.Id,
                Name = category.Name,

                Products = category.Products.Select(p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCategoryById([FromRoute] int id, [FromBody] CategoryUpdateModel model, CancellationToken token)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id, token);
            if (category == null)
                return NotFound();

            category.Name = model.Name;

            await _context.SaveChangesAsync(token);
            return Ok(category);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] int id, CancellationToken token)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id, token);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(token);
            return Ok("Category deleted");

        }


    }
}
