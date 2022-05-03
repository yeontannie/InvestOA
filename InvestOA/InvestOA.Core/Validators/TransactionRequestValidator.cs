using FluentValidation;
using InvestOA.Core.Requests;

namespace InvestOA.Core.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(buy => buy.Symbol).NotEmpty().NotNull();
            RuleFor(buy => buy.Shares).NotEmpty().NotNull();
        }
    }
}
