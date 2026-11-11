using AspNet.BLL.DTOs.ProductDetails;
using AspNet.BLL.DTOs.Tags;

namespace AspNet.BLL.DTOs.Products
{
    public class ProductFullDto : ProductDto
    {
        public ProductDetailsDto? Details { get; set; }

        public ICollection<TagDto?> Tags { get; set; } = new List<TagDto>()!;
    }
}
