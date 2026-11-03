using AspNet.Entities;

namespace AspNet.Models
{
    public class ProductUpdateModel
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
