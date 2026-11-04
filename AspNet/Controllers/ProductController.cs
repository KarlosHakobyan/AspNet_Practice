using AspNet.Models.CategoryModels;
using AspNet.Models.ProductDetailsModels;
using AspNet.Models.ProductModels;
using AspNet.Models.TagModels;
using AspNet.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService service) : ControllerBase
    {
        private readonly IProductService _service = service;

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddModel model, CancellationToken token)
        {
            await _service.CreateAsync(model, token);

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken token)
        {
            var products = await _service.GetAllAsync(token);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id, CancellationToken token)
        {
            var product = await _service.GetByIdAsync(id, token);

            if (product == null)
            return NotFound();

            return Ok(product);
        }


        [HttpGet("{id}/category")]
        public async Task<IActionResult> GetProductWithCategory([FromRoute] int id, CancellationToken token)
        {
            var product = await _service.GetWithCategoryAsync(id, token);

            if (product == null)
                return NotFound();

            var result = new ProductWithCategoryModel
            {
                Id = product.Id,
                Name = product.Name,

                Category = product.Category == null
                    ? null
                    : new CategoryModel
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    }
            };

            return Ok(result);
        }



        [HttpGet("{id}/full")]
        public async Task<IActionResult> GetFullProductInfo([FromRoute] int id, CancellationToken token)
        {
            var product = await _service.GetFullAsync(id, token);

            if (product == null)
                return NotFound();

            var result = new ProductFullModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,

                Category = product.Category == null
                    ? null
                    : new CategoryModel
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    },

                Details = product.Details == null
                    ? null
                    : new ProductDetailsModel
                    {
                        Description = product.Details.Description,
                        Manufacturer = product.Details.Manufacturer,
                        WarrantyMonths = product.Details.WarrantyMonths
                    },

                Tags = product.ProductTags.Select(pt => new TagModel
                {
                    Id = pt.Tag.Id,
                    Name = pt.Tag.Name
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] int id, CancellationToken token)
        {
            var product = await _service.GetWithDetailsAsync(id, token);

            if (product == null)
                return NotFound();

            var result = new ProductWithDetailsModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,

                Details = product.Details == null ? null : new ProductDetailsModel
                {
                    Description = product.Details.Description,
                    Manufacturer = product.Details.Manufacturer,
                    WarrantyMonths = product.Details.WarrantyMonths
                }
            };

            return Ok(result);
        }



        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProductById([FromRoute] int id, [FromBody] ProductUpdateModel model, CancellationToken token)
        {
            var product = await _service.UpdateAsync(id, model, token);

            if (product == null)
                return NotFound();
            return Ok($"Product updated\n {product}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken token)
        {
            bool result = await _service.DeleteAsync(id, token);

            if (result == false)
                return NotFound();

            return NoContent();
        }
    }


}

