using System;
using FluentValidation;

namespace Shared.Messages.DamageReports;

public sealed class CircumstancesDtoValidator : AbstractValidator<CircumstancesDto>
{
    public CircumstancesDtoValidator(PersonDtoValidator personDtoValidator)
    {
        RuleFor(x => x.DateOfAccidentUtc).NotEmpty().LessThan(DateTime.UtcNow);
        RuleFor(x => x.AccidentType).IsInEnum();
        RuleForEach(x => x.Passengers).SetValidator(personDtoValidator);
        RuleFor(x => x.CountryCode).NotEmpty().Length(2);
        RuleFor(x => x.ReasonOfTravel).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CarType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CarColor).NotEmpty().MaximumLength(50);
        When(
            x => x.OtherPartyContact != null,
            () =>
            {
                RuleFor(x => x.OtherPartyContact).SetValidator(personDtoValidator!);
            }
        );
    }
}