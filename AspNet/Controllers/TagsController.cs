
using AspNet.Entities;
using AspNet.Models;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController(ITagRepository repository, IUnitOfWork uow) : ControllerBase
    {
        private readonly ITagRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;

        [HttpGet]
        public async Task<IActionResult> GetAllTags(CancellationToken token)
        {
            var tags = await _repository.GetAllAsync(token);
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById([FromRoute] int id,CancellationToken token)
        {
            var tag = await _repository.GetByIdAsync(id, token);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] TagAddModel model,CancellationToken token)
        {
            var tag = new Tag
            {
                Name = model.Name
            };

            await _repository.AddAsync(tag, token);
            await _uow.SaveChangesAsync(token);

            return Ok( tag);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag([FromRoute] int id,[FromBody] TagUpdateModel model,CancellationToken token)
        {
            var tag = await _repository.GetByIdAsync(id, token);

            if (tag == null)
                return NotFound();

            tag.Name = model.Name;

            await _repository.UpdateAsync(tag, token);
            await _uow.SaveChangesAsync(token);

            return Ok(tag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(
            [FromRoute] int id,
            CancellationToken token)
        {
            var tag = await _repository.GetByIdAsync(id, token);

            if (tag == null)
                return NotFound();

            var isUsed = await _repository.IsUsedAsync(id, token);

            if (isUsed)
                return Conflict(
                    "Cannot delete tag because it is assigned to products.");

            await _repository.DeleteAsync(id, token);
            await _uow.SaveChangesAsync(token);

            return NoContent();
        }
    }
}
