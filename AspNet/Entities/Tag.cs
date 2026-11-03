namespace AspNet.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public ICollection<ProductTag> ProductTags { get; set; }
            = new List<ProductTag>();
    }
}
