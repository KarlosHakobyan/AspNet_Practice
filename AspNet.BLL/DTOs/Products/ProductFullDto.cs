using AspNet.Api.Models.DTOs.ProductDetails;
using AspNet.Api.Models.DTOs.Tags;

namespace AspNet.Api.Models.DTOs.Products
{
    public class ProductFullDto : ProductDto
    {
        public ProductDetailsDto? Details { get; set; }

        public ICollection<TagDto?> Tags { get; set; } = new List<TagDto>()!;
    }
}
