namespace AspNet.Models
{
    public class ProductDetailsAddModdel
    {
        public int ProductId { get; set; }

        public string? Description { get; set; }

        public string Manufacturer { get; set; } = default!;

        public int WarrantyMonths { get; set; }


    }
}
