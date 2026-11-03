namespace AspNet.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public ProductDetails? Details { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; }
    = new List<ProductTag>();
    }
}