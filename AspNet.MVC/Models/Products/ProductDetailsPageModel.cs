
using AspNet.MVC.Models.ProductDetails;

namespace AspNet.MVC.Models.Products
{
    public class ProductDetailsPageModel
    {
        public ProductModel Product { get; set; } = new();

        public ProductDetailsModel? Details { get; set; }
    }
}
