using AspNet.BLL.DTOs.Tags;
using AspNet.BLL.Services.Products;
using AspNet.Domain.Models.TagModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController(ITagService service, IMapper mapper) : ControllerBase
    {
        private readonly ITagService _service = service;
        private readonly IMapper _mapper = mapper;

    [HttpGet]
        public async Task<IActionResult> GetAllTags(CancellationToken token)
        {
            var tags = await _service.GetAllAsync(token);
            return Ok(_mapper.Map<IEnumerable<TagDto>>(tags));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById([FromRoute] int id, CancellationToken token)
        {
            var tag = await _service.GetByIdAsync(id, token);

            if (tag == null)
                return NotFound();

            return Ok(_mapper.Map<TagDto>(tag));
        }

        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] TagAddModel model, CancellationToken token)
        {
            var tag = await _service.CreateAsync(model, token);

            return Ok(_mapper.Map<TagDto>(tag)); ;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag([FromRoute] int id, [FromBody] TagUpdateModel model, CancellationToken token)
        {
            var tag = await _service.UpdateAsync(id, model, token);

            if (tag == null)
                return NotFound();

            return Ok($"Tag updated\n {_mapper.Map<TagDto>(tag)}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            bool result = await _service.DeleteAsync(id, token);

            if (result == false)
                return NotFound();

            return NoContent();
        }
    }
}
