using AspNet.Entities;
using AspNet.Models;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductRepository repository, IUnitOfWork uow) : ControllerBase
    {
        private readonly IProductRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductAddModel model, CancellationToken token)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId
            };
            await _repository.AddAsync(product, token);
            await _uow.SaveChangesAsync(token);

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken token)
        {
            var products = await _repository.GetAllAsync(token);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id, CancellationToken token)
        {
            var product = await _repository.GetByIdAsync(id, token);

            if (product == null)
            return NotFound();

            return Ok(product);
        }


        [HttpGet("{id}/category")]
        public async Task<IActionResult> GetProductWithCategory([FromRoute] int id, CancellationToken token)
        {
            var product = await _repository.GetWithCategoryAsync(id, token);

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
            var product = await _repository.GetFullAsync(id, token);

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
            var product = await _repository.GetWithDetailsAsync(id, token);

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
            var product = await _repository.GetByIdAsync(id,token);
            if (product == null)
                return NotFound();

            product.Price = model.Price;
            product.Name = model.Name;

            await _repository.UpdateAsync(product, token);
            await _uow.SaveChangesAsync(token);

            return Ok($"Product updated\n {product}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken token)
        {
            var product = await _repository.GetByIdAsync(id, token);

            if (product == null)
                return NotFound();

            await _repository.DeleteAsync(id, token);
            await _uow.SaveChangesAsync(token);

            return NoContent();
        }
    }


}

