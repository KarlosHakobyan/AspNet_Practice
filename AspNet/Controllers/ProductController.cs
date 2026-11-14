
using AspNet.Api.Extensions;

using AspNet.BLL.DTOs.Products;
using AspNet.BLL.Services.Products;

using AspNet.Data.Identity;

using AspNet.Domain.Models.ProductModels;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IProductService service,
    IMapper mapper) : ControllerBase
{
    private readonly IProductService _service = service;
    private readonly IMapper _mapper = mapper;

    // ADMIN + MANAGER
    [Authorize(Roles = RoleNames.Admin + "," + RoleNames.Manager)]
    [HttpPost]
    public async Task<IActionResult> AddProduct(
        [FromBody] ProductAddModel model,
        CancellationToken token)
    {
        var result = await _service.CreateAsync(model, token);

        return this.ToActionResult(
            result,
            product =>
            {
                var dto = _mapper.Map<ProductDto>(product);

                return CreatedAtAction(
                    nameof(GetProductById),
                    new { id = product.Id },
                    dto);
            });
    }

    // CUSTOMER + MANAGER + ADMIN
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        CancellationToken token)
    {
        var result = await _service.GetAllAsync(token);

        return this.ToActionResult(
            result,
            products => Ok(
                _mapper.Map<IEnumerable<ProductDto>>(products)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetByIdAsync(id, token);

        return this.ToActionResult(
            result,
            product => Ok(_mapper.Map<ProductDto>(product)));
    }

    [HttpGet("{id:int}/category")]
    public async Task<IActionResult> GetProductWithCategory(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetWithCategoryAsync(id, token);

        return this.ToActionResult(
            result,
            product => Ok(_mapper.Map<ProductDto>(product)));
    }

    [HttpGet("{id:int}/full")]
    public async Task<IActionResult> GetFullProductInfo(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetFullAsync(id, token);

        return this.ToActionResult(
            result,
            product => Ok(_mapper.Map<ProductFullDto>(product)));
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetProductWithDetails(
        [FromRoute] int id,
        CancellationToken token)
    {
        var result = await _service.GetWithDetailsAsync(id, token);

        return this.ToActionResult(
            result,
            product => Ok(_mapper.Map<ProductWithDetailsDto>(product)));
    }

    // ADMIN + MANAGER
    [Authorize(Roles = RoleNames.Admin + "," + RoleNames.Manager)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductById(
        [FromRoute] int id,
        [FromBody] ProductUpdateModel model,
        CancellationToken token)
    {
        var result = await _service.UpdateAsync(id, model, token);

        return this.ToActionResult(
            result,
            product => Ok(_mapper.Map<ProductDto>(product)));
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
