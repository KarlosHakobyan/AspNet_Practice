using AspNet.BLL.DTOs.Categories;
using AspNet.BLL.Services.Products;
using AspNet.Domain.Models.CategoryModels;
using AspNet.Domain.Models.ProductModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService service, IMapper mapper) : ControllerBase
    {
        private readonly ICategoryService _service = service;
        private readonly IMapper _mapper = mapper;


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

            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken token)
        {
            var category = await _service.GetByIdAsync(id, token);

            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<CategoryDto>(category));
        }


        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts([FromRoute] int id, CancellationToken token)
        {
            var category = await _service.GetWithProductsAsync(id, token);

            if (category == null)
                return NotFound();
                return Ok(_mapper.Map<CategoryWithProductsDto>(category));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryById([FromRoute] int id, [FromBody] CategoryUpdateModel model, CancellationToken token)
        {
            var category = await _service.UpdateAsync(id, model, token);

            if (category == null)
                return NotFound();
                return Ok($"Category updated\n {_mapper.Map<CategoryDto>(category)}");
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
