
using AspNet.Domain.Models.ProductModels;
using FluentValidation;

namespace AspNet.BLL.Validators.Products;

public class ProductAddModelValidator : AbstractValidator<ProductAddModel>
{
    public ProductAddModelValidator()
    {
        // Name validation
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Name is required.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Name cannot contain only whitespace.")
            .MinimumLength(2)
                .WithMessage("Name must contain at least 2 characters.")
            .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.");

        // Price validation
        RuleFor(x => x.Price)
            .InclusiveBetween(500, 1000000)
            .WithMessage("Price must be between 500 and 1000000.");

        // Category validation
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than zero.");

        // Expensive products must belong to category 1 or 2
        When(x => x.Price > 10000, () =>
        {
            RuleFor(x => x.CategoryId)
                .Must(id => id == 1 || id == 2)
                .WithMessage("Expensive products must belong to category 1 or 2.");
        });

        // Expensive products must have a name of at least 5 characters
        When(x => x.Price > 100000, () =>
        {
            RuleFor(x => x.Name)
                .MinimumLength(5)
                .WithMessage("Expensive products must have a name of at least 5 characters.");
        });

        // Apple products must cost at least 100
        When(x => !string.IsNullOrWhiteSpace(x.Name) &&
                  x.Name.Contains("Apple", StringComparison.OrdinalIgnoreCase), () =>
                  {
                      RuleFor(x => x.Price)
                          .GreaterThanOrEqualTo(100)
                          .WithMessage("Apple products must cost at least 100.");
                  });
    }
}
