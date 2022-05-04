using FluentValidation;
using InvestOA.Core.Requests;

namespace InvestOA.Core.Validators
{
    public class IndexRequestValidator : AbstractValidator<IndexRequest>
    {
        public IndexRequestValidator()
        {
            RuleFor(i => i.Cash).NotEmpty().NotNull();
            RuleFor(i => i.Total).NotEmpty().NotNull();
        }
    }
}
