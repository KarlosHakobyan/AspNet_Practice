namespace AspNet.Api.Models.DTOs.ProductDetails
{
    public class ProductDetailsDto
    {
        public string? Description { get; set; }

        public string Manufacturer { get; set; } = default!;

        public int WarrantyMonths { get; set; }
    }
}
