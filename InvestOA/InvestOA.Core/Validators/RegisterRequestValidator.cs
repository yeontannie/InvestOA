using FluentValidation;
using InvestOA.Core.Requests;

namespace InvestOA.Core.Validators
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
