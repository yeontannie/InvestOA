using FluentValidation;
using InvestOA.Core.Requests;

namespace InvestOA.Core.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(reg => reg.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(reg => reg.Password).NotEmpty().NotNull();
        }
    }
}
