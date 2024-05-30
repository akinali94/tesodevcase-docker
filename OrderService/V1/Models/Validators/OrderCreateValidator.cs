using FluentValidation;
using OrderService.V1.Models.CommandModels;

namespace OrderService.V1.Models.Validators;

public class OrderCreateValidator : AbstractValidator<CreateCommand>
{
    public OrderCreateValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer Id is required");
        
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required")
            .GreaterThan(0).WithMessage("Quantity should be more than 0")
            .InclusiveBetween(1,20).WithMessage("You can add no more than 20 product");
        
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price should be greater than 0");
        
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required");
        
        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Product is required");
        
    }
}