namespace AspNet.Domain.Models.ProductDetailsModels
{
    public class ProductDetailsAddModel
    {
        public int ProductId { get; set; }

        public string? Description { get; set; }

        public string Manufacturer { get; set; } = default!;

        public int WarrantyMonths { get; set; }


    }
}
