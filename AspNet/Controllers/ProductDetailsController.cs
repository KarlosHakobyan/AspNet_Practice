
using AspNet.Api.Extensions;

using AspNet.BLL.DTOs.ProductDetails;
using AspNet.BLL.Services.Products;

using AspNet.Data.Identity;

using AspNet.Domain.Models.ProductDetailsModels;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductDetailsController(
    IProductDetailsService service,
    IMapper mapper) : ControllerBase
{
    private readonly IProductDetailsService _service = service;
    private readonly IMapper _mapper = mapper;

    // CUSTOMER + MANAGER + ADMIN
    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetById(
        [FromRoute] int productId,
        CancellationToken token)
    {
        var result = await _service.GetByIdAsync(productId, token);

        return this.ToActionResult(
            result,
            details => Ok(_mapper.Map<ProductDetailsDto>(details))
        );
    }

    // ADMIN + MANAGER
    [Authorize(Roles = RoleNames.Admin + "," + RoleNames.Manager)]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] ProductDetailsAddModel model,
        CancellationToken token)
    {
        var result = await _service.CreateAsync(model, token);

        return this.ToActionResult(
            result,
            details => CreatedAtAction(
                nameof(GetById),
                new { productId = details.ProductId },
                _mapper.Map<ProductDetailsDto>(details)
            )
        );
    }

    // ADMIN + MANAGER
    [Authorize(Roles = RoleNames.Admin + "," + RoleNames.Manager)]
    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int productId,
        [FromBody] ProductDetailsUpdateModel model,
        CancellationToken token)
    {
        var result = await _service.UpdateAsync(
            productId,
            model,
            token
        );

        return this.ToActionResult(
            result,
            details => Ok(_mapper.Map<ProductDetailsDto>(details))
        );
    }

    // ADMIN ONLY
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int productId,
        CancellationToken token)
    {
        var result = await _service.DeleteAsync(productId, token);

        return this.ToActionResult(result);
    }
}
