using AspNet.Domain.Models.CategoryModels;
using AspNet.Domain.Models.ProductDetailsModels;
using AspNet.Domain.Models.TagModels;

namespace AspNet.Domain.Models.ProductModels
{
    public class ProductFullModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public CategoryModel? Category { get; set; }

        public ProductDetailsModel? Details { get; set; }

        public List<TagModel> Tags { get; set; } = new();
    }
}