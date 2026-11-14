
using AspNet.Api.Extensions;

using AspNet.BLL.DTOs.Categories;
using AspNet.BLL.Services.Products;

using AspNet.Data.Identity;

using AspNet.Domain.Models.CategoryModels;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController(
    ICategoryService service,
    IMapper mapper) : ControllerBase
{
    private readonly ICategoryService _service = service;
    private readonly IMapper _mapper = mapper;

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public async Task<IActionResult> AddCategory(
        [FromBody] CategoryAddModel model,
        CancellationToken token)
    {
        var result = await _service.CreateAsync(model, token);

        return this.ToActionResult(
            result,
            category => CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.Id },
                _mapper.Map<CategoryDto>(category)
            )
        );
    }

    // CUSTOMER + MANAGER + ADMIN
    [HttpGet]
    public async Task<IActionResult> GetCategories(
        CancellationToken token)
    {
        var result = await _service.GetAllAsync(token);

        return this.ToActionResult(
            result,
            categories => Ok(
                _mapper.Map<IEnumerable<CategoryDto>>(categories)
            )
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetByIdAsync(id, token);

        return this.ToActionResult(
            result,
            category => Ok(_mapper.Map<CategoryDto>(category))
        );
    }

    [HttpGet("{id:int}/products")]
    public async Task<IActionResult> GetCategoryWithProducts(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetWithProductsAsync(id, token);

        return this.ToActionResult(
            result,
            category => Ok(
                _mapper.Map<CategoryWithProductsDto>(category)
            )
        );
    }

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategoryById(
        [FromRoute] int id,
        [FromBody] CategoryUpdateModel model,
        CancellationToken token)
    {
        var result = await _service.UpdateAsync(id, model, token);

        return this.ToActionResult(
            result,
            category => Ok(_mapper.Map<CategoryDto>(category))
        );
    }

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategoryById(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.DeleteAsync(id, token);

        return this.ToActionResult(result);
    }
}
