using FluentValidation;

namespace Shared.Paging;

public sealed class PagingParametersValidator : AbstractValidator<PagingParameters>
{
    public PagingParametersValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 100);
    }
}