namespace AspNet.Models.ProductDetailsModels
{
    public class ProductDetailsUpdateModel
    {
        public string? Description { get; set; }

        public string Manufacturer { get; set; } = default!;

        public int WarrantyMonths { get; set; }
    }
}
