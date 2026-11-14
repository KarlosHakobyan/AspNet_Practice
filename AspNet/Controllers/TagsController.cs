
using AspNet.Api.Extensions;

using AspNet.BLL.DTOs.Tags;
using AspNet.BLL.Services.Products;

using AspNet.Data.Identity;

using AspNet.Domain.Models.TagModels;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagsController(
    ITagService service,
    IMapper mapper) : ControllerBase
{
    private readonly ITagService _service = service;
    private readonly IMapper _mapper = mapper;

    // CUSTOMER + MANAGER + ADMIN
    [HttpGet]
    public async Task<IActionResult> GetAllTags(
        CancellationToken token)
    {
        var result = await _service.GetAllAsync(token);

        return this.ToActionResult(
            result,
            tags => Ok(_mapper.Map<IEnumerable<TagDto>>(tags))
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTagById(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetByIdAsync(id, token);

        return this.ToActionResult(
            result,
            tag => Ok(_mapper.Map<TagDto>(tag))
        );
    }

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public async Task<IActionResult> AddTag(
        [FromBody] TagAddModel model,
        CancellationToken token)
    {
        var result = await _service.CreateAsync(model, token);

        return this.ToActionResult(
            result,
            tag => CreatedAtAction(
                nameof(GetTagById),
                new { id = tag.Id },
                _mapper.Map<TagDto>(tag)
            )
        );
    }

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTag(
        [FromRoute] int id,
        [FromBody] TagUpdateModel model,
        CancellationToken token)
    {
        var result = await _service.UpdateAsync(id, model, token);

        return this.ToActionResult(
            result,
            tag => Ok(_mapper.Map<TagDto>(tag))
        );
    }

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.DeleteAsync(id, token);

        return this.ToActionResult(result);
    }
}
