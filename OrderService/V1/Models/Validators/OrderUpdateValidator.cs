using FluentValidation;
using OrderService.V1.Models.CommandModels;

namespace OrderService.V1.Models.Validators;

public class OrderUpdateValidator : AbstractValidator<UpdateCommand>
{
    public OrderUpdateValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer Id can not be empty");
        
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity can not be empty")
            .GreaterThan(0).WithMessage("Quantity should be more than 0")
            .InclusiveBetween(1,20).WithMessage ("You can add no more than 20 product");
        
        RuleFor(x => x.Addresses)
            .NotEmpty().WithMessage("Address can not be empty");
        
        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Products can not be empty");
    }
}