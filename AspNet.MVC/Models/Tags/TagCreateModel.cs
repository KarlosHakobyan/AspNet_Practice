using System.ComponentModel.DataAnnotations;

namespace AspNet.MVC.Models.Tags
{
    public class TagCreateModel
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}