using AspNet.Models.ProductDetailsModels;
using AspNet.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController(
        IProductDetailsService service) : ControllerBase
    {
        private readonly IProductDetailsService _service = service;

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int productId,
            CancellationToken token)
        {
            var details = await _service.GetByIdAsync(productId, token);

            if (details == null)
                return NotFound();

            return Ok(details);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ProductDetailsAddModel model,
            CancellationToken token)
        {
            var details = await _service.CreateAsync(model, token);

            return CreatedAtAction(
                nameof(GetById),
                new { productId = details!.ProductId },
                details);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(
            [FromRoute] int productId,
            [FromBody] ProductDetailsUpdateModel model,
            CancellationToken token)
        {
            var details = await _service.UpdateAsync(productId, model, token);

            if (details == null)
                return NotFound();

            return Ok(details);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int productId,
            CancellationToken token)
        {
            var result = await _service.DeleteAsync(productId, token);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}