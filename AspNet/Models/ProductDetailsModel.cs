namespace AspNet.Models
{
    public class ProductDetailsModel
    {
        public string Description { get; set; } = default;

        public string Manufacturer { get; set; } = default;

        public int WarrantyMonths { get; set; }
    }
}