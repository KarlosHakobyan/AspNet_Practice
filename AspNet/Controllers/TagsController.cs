using AspNet.Data;
using AspNet.Entities;
using AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] Tag model,CancellationToken token)
        {
            var exists = await _context.Tags.AnyAsync(t => t.Name == model.Name, token);

            if (exists)
                return Conflict("Tag already exists.");

            var tag = new Tag
            {
                Name = model.Name
            };

            await _context.Tags.AddAsync(tag, token);
            await _context.SaveChangesAsync(token);

            return Ok(tag);
           
        }

        [HttpGet]
        public async Task<IActionResult> GetTags(CancellationToken token)
        {
            var tags = await _context.Tags.ToListAsync(token);

            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById([FromRoute] int id,CancellationToken token)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id, token);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag([FromRoute] int id,[FromBody] TagUpdateModel model,CancellationToken token)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id, token);

            if (tag == null)
                return NotFound();

            var duplicate = await _context.Tags.AnyAsync(t => t.Name == model.Name && t.Id != id,token);

            if (duplicate)
                return Conflict("Another tag with this name already exists.");

            tag.Name = model.Name;

            await _context.SaveChangesAsync(token);

            return Ok(tag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken token)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id, token);

            if (tag == null)
                return NotFound();

            var isUsed = await _context.ProductTags.AnyAsync(pt => pt.TagId == id, token);

            if (isUsed)
                return Conflict("Cannot delete this tag because it is assigned to products.");

            _context.Tags.Remove(tag);

            await _context.SaveChangesAsync(token);

            return NoContent();
        }


    }
}