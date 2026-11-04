using AspNet.Entities;
using AspNet.Models;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTagsController(IProductTagRepository repository, IUnitOfWork uow) : ControllerBase
    {
        private readonly IProductTagRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var productTags = await _repository.GetAllAsync(token);

            return Ok(productTags);
        }

        [HttpGet("{productId}/{tagId}")]
        public async Task<IActionResult> GetByIds([FromRoute] int productId,[FromRoute] int tagId,CancellationToken token)
        {
            var productTag = await _repository.GetByIdsAsync(productId, tagId, token);

            if (productTag == null)
                return NotFound();

            return Ok(productTag);
        }

       
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId([FromRoute] int productId,CancellationToken token)
        {
            var productTags = await _repository.GetByProductIdAsync(productId, token);

            return Ok(productTags);
        }

        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductTagsAddModel model,CancellationToken token)
        {
            if (await _repository.ExistsAsync(model.ProductId, model.TagId, token))
            {
                return Conflict("Relationship already exists.");
            }

            var productTag = new ProductTag
            {
                ProductId = model.ProductId,
                TagId = model.TagId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(productTag, token);
            await _uow.SaveChangesAsync(token);

            return Ok(productTag);
        }

        
        [HttpDelete("{productId}/{tagId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId,[FromRoute] int tagId,CancellationToken token)
        {
            var exists = await _repository.ExistsAsync(productId, tagId, token);

            if (!exists)
                return NotFound();

            await _repository.DeleteAsync(productId, tagId, token);
            await _uow.SaveChangesAsync(token);

            return NoContent();
        }
    }
}
