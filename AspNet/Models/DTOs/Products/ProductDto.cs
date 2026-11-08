using AspNet.Models.DTOs.Categories;

namespace AspNet.Models.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public CategoryDto? Category { get; set; }
    }
}
