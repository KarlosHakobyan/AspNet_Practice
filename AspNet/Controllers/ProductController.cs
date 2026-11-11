using AspNet.BLL.DTOs.Products;
using AspNet.BLL.Services.Products;
using AspNet.Domain.Models.ProductModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService service,IMapper mapper) : ControllerBase
    {
        private readonly IProductService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddModel model,CancellationToken token)
        {
            var result = await _service.CreateAsync(model, token);

            if (!result)
                return BadRequest("Category does not exist.");

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken token)
        {
            var products = await _service.GetAllAsync(token);

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id,CancellationToken token)
        {
            var product = await _service.GetByIdAsync(id, token);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpGet("{id}/category")]
        public async Task<IActionResult> GetProductWithCategory([FromRoute] int id,CancellationToken token)
        {
            var product = await _service.GetWithCategoryAsync(id, token);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpGet("{id}/full")]
        public async Task<IActionResult> GetFullProductInfo([FromRoute] int id,CancellationToken token)
        {
            var product = await _service.GetFullAsync(id, token);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductFullDto>(product));
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] int id,CancellationToken token)
        {
            var product = await _service.GetWithDetailsAsync(id, token);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<ProductWithDetailsDto>(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductById([FromRoute] int id,[FromBody] ProductUpdateModel model,CancellationToken token)
        {
            var result = await _service.UpdateAsync(id, model, token);

            if (!result)
                return BadRequest("Product or Category does not exist.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken token)
        {
            var result = await _service.DeleteAsync(id, token);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
