namespace AspNet.MVC.Models.Products
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public CategoryModel? Category { get; set; }

        public int CategoryId { get; set; }
    }

    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}