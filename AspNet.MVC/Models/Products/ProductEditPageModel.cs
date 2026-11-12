
using AspNet.MVC.Models.ProductDetails;

namespace AspNet.MVC.Models.Products
{
    public class ProductEditPageModel
    {
        public ProductUpdateModel Product { get; set; } = new();

        public ProductDetailsUpdateModel Details { get; set; } = new();

        public bool HasDetails { get; set; }

        public string? ManufacturerOption { get; set; }

        public string? CustomManufacturer { get; set; }
    }
}
