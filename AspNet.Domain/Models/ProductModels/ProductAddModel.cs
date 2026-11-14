using AspNet.Domain.Validations.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AspNet.Domain.Models.ProductModels
{
    public class ProductAddModel // : IValidatableObject
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        /* public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
         {
             if (string.IsNullOrWhiteSpace(Name))
             {
                 yield return new ValidationResult("Name cannot contain only whitespace.", [nameof(Name)]);
             }

             if (Price > 100000 && !string.IsNullOrWhiteSpace(Name) && Name.Length < 5)
             {
                 yield return new ValidationResult("Expensive products must have a name of at least 5 characters.", [nameof(Name), nameof(Price)]);
             }

             if (Price > 50000 && CategoryId != 1 && CategoryId != 2)
             {
                 yield return new ValidationResult("CategoryId must be 1 or 2 when Price > 50000.", [nameof(CategoryId)]);
             }

             if (!string.IsNullOrWhiteSpace(Name) && Name.Contains("Apple", StringComparison.OrdinalIgnoreCase) && Price < 100)
             {
                 yield return new ValidationResult("Price must be at least 100 when the Name contains 'Apple'.", [nameof(Price)]);
             }
         }*/
    }
}