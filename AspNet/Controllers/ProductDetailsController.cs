using AspNet.Api.Extensions;
using AspNet.BLL.DTOs.ProductDetails;
using AspNet.BLL.Services.Products;
using AspNet.Domain.Models.ProductDetailsModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductDetailsController(IProductDetailsService service,IMapper mapper) : ControllerBase
{
    private readonly IProductDetailsService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetById([FromRoute] int productId,CancellationToken token)
    {
        var result = await _service.GetByIdAsync(productId, token);

        return this.ToActionResult(result,details => Ok(_mapper.Map<ProductDetailsDto>(details)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDetailsAddModel model,CancellationToken token)
    {
        var result = await _service.CreateAsync(model, token);

        return this.ToActionResult(result,details => CreatedAtAction(nameof(GetById),new { productId = details.ProductId },_mapper.Map<ProductDetailsDto>(details)));
    }

    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Update([FromRoute] int productId,[FromBody] ProductDetailsUpdateModel model,CancellationToken token)
    {
        var result = await _service.UpdateAsync(productId,model,token);

        return this.ToActionResult(result,details => Ok(_mapper.Map<ProductDetailsDto>(details)));
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Delete([FromRoute] int productId,CancellationToken token)
    {
        var result = await _service.DeleteAsync(productId, token);

        return this.ToActionResult(result);
    }
}