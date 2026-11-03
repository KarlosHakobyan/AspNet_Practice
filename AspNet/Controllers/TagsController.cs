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
        public async Task<IActionResult> AddTag([FromBody] TagAddModel model,CancellationToken token)
        {
            var tag = new Tag
            {
                Name = model.Name
            };

            _context.Tags.Add(tag);

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


    }
}