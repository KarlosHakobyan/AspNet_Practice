using AspNet.Entities;

namespace AspNet.Models.ProductModels
{
    public class ProductAddModel
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
