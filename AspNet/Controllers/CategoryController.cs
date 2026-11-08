using AspNet.Api.Services.Products;
using AspNet.Domain.Models.CategoryModels;
using AspNet.Domain.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        private readonly ICategoryService _service = service;


        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryAddModel model, CancellationToken token)
        {
            await _service.CreateAsync(model, token);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken token)
        {
            var categories = await _service.GetAllAsync(token);
            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken token)
        {
            var category = await _service.GetByIdAsync(id, token);

            if (category == null)
                return NotFound();

            return Ok(category);
        }


        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts([FromRoute] int id, CancellationToken token)
        {
            var category = await _service.GetWithProductsAsync(id, token);

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
            var category = await _service.UpdateAsync(id, model, token);

            if (category == null)
                return NotFound();
            return Ok($"Category updated\n {category}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] int id, CancellationToken token)
        {
            bool result = await _service.DeleteAsync(id, token);

            if (result == false)
                return NotFound();

            return NoContent();
        }
    }
}
