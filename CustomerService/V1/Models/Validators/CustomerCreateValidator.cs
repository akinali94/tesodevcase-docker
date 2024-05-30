using CustomerService.V1.Models.RequestModels;
using FluentValidation;

namespace CustomerService.V1.Models.Validators;

public class CustomerCreateValidator : AbstractValidator<CustomerCreateModel>
{
    public CustomerCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 25).WithMessage("Name must be between 2 and 25 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Addresses[0].AddressLine)
            .NotEmpty().WithMessage("AddressLine is required.")
            .Length(2, 25).WithMessage("AddressLine must be between 2 and 25 characters.");

        RuleFor(x => x.Addresses[0].Country)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(x => x.Addresses[0].City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.Addresses[0].CityCode)
            .InclusiveBetween(10000, 100000).WithMessage("CityCode must be between 10000 and 100000.");
    }
    
}