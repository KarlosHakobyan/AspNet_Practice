
using AspNet.Entities;
using AspNet.Models;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController(
        IProductDetailsRepository repository , IUnitOfWork uow) : ControllerBase
    {
        private readonly IProductDetailsRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProductId([FromRoute] int productId,CancellationToken token)
        {
            var details = await _repository.GetByIdAsync(productId, token);

            if (details == null)
                return NotFound();

            return Ok(details);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddDetail([FromBody] ProductDetailsAddModel model,
            CancellationToken token)
        {
            var existing = await _repository.GetByIdAsync(model.ProductId, token);

            if (existing != null)
                return Conflict("Product details already exist.");

            var details = new ProductDetails
            {
                ProductId = model.ProductId,
                Description = model.Description,
                Manufacturer = model.Manufacturer,
                WarrantyMonths = model.WarrantyMonths
            };

            await _repository.AddAsync(details, token);
            await _uow.SaveChangesAsync(token);

            return Ok(details);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update([FromRoute] int productId,[FromBody] ProductDetailsUpdateModel model,CancellationToken token)
        {
            var details = await _repository.GetByIdAsync(productId, token);

            if (details == null)
                return NotFound();

            details.Description = model.Description;
            details.Manufacturer = model.Manufacturer;
            details.WarrantyMonths = model.WarrantyMonths;

            await _repository.UpdateAsync(details, token);
            await _uow.SaveChangesAsync(token);

            return Ok(details);
        }

      
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId,CancellationToken token)
        {
            var details = await _repository.GetByIdAsync(productId, token);

            if (details == null)
                return NotFound();

            await _repository.DeleteAsync(productId, token);
            await _uow.SaveChangesAsync(token);

            return NoContent();
        }
    }
}
