using AspNet.Models.CategoryModels;
using AspNet.Models.ProductDetailsModels;
using AspNet.Models.TagModels;

namespace AspNet.Models.ProductModels
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