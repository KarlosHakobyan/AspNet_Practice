using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.ProductDetails
{
    public class ProductDetailsCreateModel
    {
        [Required]
        public int ProductId { get; set; }

        public string? Description { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Manufacturer { get; set; } = string.Empty;

        [Range(0, 120)]
        public int WarrantyMonths { get; set; }
    }
}