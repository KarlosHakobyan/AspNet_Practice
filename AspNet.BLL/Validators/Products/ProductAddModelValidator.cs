using AspNet.Domain.Models.ProductModels;
using FluentValidation;

namespace AspNet.BLL.Validators.Products
{
    public class ProductAddModelValidator : AbstractValidator<ProductAddModel>
    {
        public ProductAddModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                    .MinimumLength(2).WithMessage("Name must contain at least 2 characters.")
                        .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                     .WithMessage("Price must be greater than zero.");

            When(x => x.Price > 50000, () =>
            {
                RuleFor(x => x.CategoryId)
                    .Must(id => id == 1 || id == 2)
                        .WithMessage("Expensive products must belong to category 1 or 2.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Name) && x.Name.Contains("Apple", StringComparison.OrdinalIgnoreCase), () =>
          {
              RuleFor(x => x.Price)
                  .GreaterThanOrEqualTo(100)
                    .WithMessage("Apple products must cost at least 100.");
          });
        }
    }
}