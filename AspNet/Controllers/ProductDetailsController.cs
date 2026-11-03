using AspNet.Data;
using AspNet.Entities;
using AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddDetails([FromBody] ProductDetailsAddModdel model, CancellationToken token)
        {
            var detailExists = await _context.Products.AnyAsync(d => d.Id == model.ProductId);

            if (!detailExists)
            {
                return BadRequest("Product does not exist.");
            }

            var detail = new ProductDetails
            {
                Description = model.Description,
                Manufacturer = model.Manufacturer,
                WarrantyMonths = model.WarrantyMonths,
            };

            _context.ProductDetails.Add(detail);
            await _context.SaveChangesAsync(token);
            return Ok(detail);

        }

                
        [HttpGet]
        public async Task<IActionResult> GetDetails(CancellationToken token)
        {
            var details = await _context.ProductDetails.ToListAsync(token);
            return Ok(details);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetails(
            [FromRoute] int id,
            [FromBody] ProductDetailsUpdateModel model,
            CancellationToken token)
        {
            var detail = await _context.ProductDetails
                .FirstOrDefaultAsync(d => d.ProductId == id, token);

            if (detail == null)
                return NotFound("Product details not found.");

            detail.Description = model.Description;
            detail.Manufacturer = model.Manufacturer;
            detail.WarrantyMonths = model.WarrantyMonths;

            await _context.SaveChangesAsync(token);

            return Ok(detail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetails([FromRoute] int id,CancellationToken token)
        {
            var detail = await _context.ProductDetails.FirstOrDefaultAsync(d => d.ProductId == id, token);

            if (detail == null)
                return NotFound();

            _context.ProductDetails.Remove(detail);
            return Ok("Details deleted");
        }

    }
}
