using FluentValidation;
using InvestOA.WebApp.Models.Requests;

namespace InvestOA.WebApp.Models.Validators
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
