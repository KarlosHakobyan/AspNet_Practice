
using AspNet.Entities;
using AspNet.Models;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryRepository repository, IUnitOfWork uow) : ControllerBase 
    {
        private readonly ICategoryRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;


        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryAddModel model,CancellationToken token)
        {
            var category = new Category
            {
                Name = model.Name
            };

            await _repository.AddAsync(category, token);
            await _uow.SaveChangesAsync(token);

            return Ok(category);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken token)
        {
            var categories = await _repository.GetAllAsync(token);

            return Ok(categories);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id,CancellationToken token)
        {
            var category = await _repository.GetByIdAsync(id, token);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

      
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts([FromRoute] int id,CancellationToken token)
        {
            var category = await _repository.GetWithProductsAsync(id, token);

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
        public async Task<IActionResult> UpdateCategoryById([FromRoute] int id,[FromBody] CategoryUpdateModel model,CancellationToken token)
        {
            var category = await _repository.GetByIdAsync(id, token);

            if (category == null)
                return NotFound();

            category.Name = model.Name;

            await _repository.UpdateAsync(category, token);
            await _uow.SaveChangesAsync(token);

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] int id,CancellationToken token)
        {
            var category = await _repository.GetByIdAsync(id, token);

            if (category == null)
                return NotFound();

            await _repository.DeleteAsync(id, token);
            await _uow.SaveChangesAsync(token);

            return NoContent();
        }
    }
}
