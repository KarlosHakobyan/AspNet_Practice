
using AspNet.BLL.DTOs.ProductDetails;
using AspNet.BLL.Services.Products;
using AspNet.Domain.Models.ProductDetailsModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController(IProductDetailsService service,IMapper mapper) : ControllerBase
    {
        private readonly IProductDetailsService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById([FromRoute] int productId,CancellationToken token)
        {
            var details = await _service.GetByIdAsync(productId, token);

            if (details == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDetailsDto>(details));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDetailsAddModel model,CancellationToken token)
        {
            var details = await _service.CreateAsync(model, token);

            if (details == null)
                return BadRequest("Product does not exist or details already exist.");

            var dto = _mapper.Map<ProductDetailsDto>(details);

            return CreatedAtAction(nameof(GetById),new { productId = details.ProductId },dto);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update([FromRoute] int productId,[FromBody] ProductDetailsUpdateModel model,CancellationToken token)
        {
            var details = await _service.UpdateAsync(productId, model, token);

            if (details == null)
                return NotFound();

            return Ok(_mapper.Map<ProductDetailsDto>(details));
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId,CancellationToken token)
        {
            var result = await _service.DeleteAsync(productId, token);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
