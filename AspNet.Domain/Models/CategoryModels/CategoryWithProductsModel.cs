using AspNet.Domain.Models.ProductModels;

namespace AspNet.Domain.Models.CategoryModels
{
    public class CategoryWithProductsModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public List<ProductModel> Products { get; set; }

    }
}
