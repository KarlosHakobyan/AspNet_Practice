using AspNet.Entities;
using AspNet.Models.DTOs.ProductDetails;
using AspNet.Models.DTOs.Tags;

namespace AspNet.Models.DTOs.Products
{
    public class ProductFullDto : ProductDto
    {      
        public ProductDetailsDto? Details { get; set; }

        public ICollection<TagDto?> Tags { get; set; } = new List<TagDto>()!;
    }
}
