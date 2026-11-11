using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.Products
{
    public class ProductCreateModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Range(500, 10000, ErrorMessage = "Price must be between 500 and 10000.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Category ID.")]
        public int CategoryId { get; set; }
    }
}