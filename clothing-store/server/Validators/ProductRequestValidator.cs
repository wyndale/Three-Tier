using FluentValidation;
using server.DTOs;

namespace server.Validators;

public class ProductRequestValidator : AbstractValidator<ProductsDTO>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(300).WithMessage("Product description cannot exceed 300 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than 0.");

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Product category is required.");
    }
}
