using AspNet.Models.CategoryModels;

namespace AspNet.Models.ProductModels
{
    public class ProductWithCategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public CategoryModel Category { get; set; }
    }
}
