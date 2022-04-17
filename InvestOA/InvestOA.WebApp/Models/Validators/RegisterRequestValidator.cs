using FluentValidation;
using InvestOA.WebApp.Models.Requests;

namespace InvestOA.WebApp.Models.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(reg => reg.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(reg => reg.Password).NotEmpty().NotNull();
            RuleFor(reg => reg.Confirmation).NotEmpty().NotNull().Matches(reg => reg.Password);
        }
    }
}
