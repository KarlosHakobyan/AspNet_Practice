namespace AspNet.Models
{
    public class CategoryWithProductsModel
    {
            public int Id { get; set; }

            public string Name { get; set; } = default!;

            public List<ProductModel> Products { get; set; }
        
    }
}
