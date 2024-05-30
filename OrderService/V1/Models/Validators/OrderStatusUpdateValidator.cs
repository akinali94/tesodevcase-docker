using FluentValidation;
using OrderService.V1.Models.CommandModels;

namespace OrderService.V1.Models.Validators;

public class OrderStatusUpdateValidator : AbstractValidator<ChangeStatusCommand>
{
    public OrderStatusUpdateValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Id can not be empty");
        RuleFor(x => x.NewStatus)
            .NotEmpty().WithMessage("Status can not be empty")
            .MinimumLength(4).WithMessage("Status can not be less than 4 characters")
            .MaximumLength(20).WithMessage("Status can not be more than 20 characters");
    }
}