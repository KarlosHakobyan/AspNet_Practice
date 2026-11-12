namespace AspNet.MVC.Models.ProductDetails
{
    public class ProductDetailsModel
    {
        public string? Description { get; set; }

        public string Manufacturer { get; set; } = string.Empty;

        public int WarrantyMonths { get; set; }
    }
}