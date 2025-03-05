using FluentValidation;
using server.DTOs;

namespace server.Validators;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductDTO>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Product description cannot exceed 300 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than 0.");
    }
}
