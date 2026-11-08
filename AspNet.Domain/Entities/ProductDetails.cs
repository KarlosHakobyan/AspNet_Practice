namespace AspNet.Domain.Entities
{
    public class ProductDetails
    {

        public int ProductId { get; set; }

        public string? Description { get; set; }

        public string Manufacturer { get; set; } = default!;

        public int WarrantyMonths { get; set; }

        public Product Product { get; set; } = null!;
    }
}
