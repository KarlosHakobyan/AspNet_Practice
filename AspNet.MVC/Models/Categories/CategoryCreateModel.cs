using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.Categories
{
    public class CategoryCreateModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}