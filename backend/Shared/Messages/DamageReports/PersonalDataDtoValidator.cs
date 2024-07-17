using System;
using FluentValidation;

namespace Shared.Messages.DamageReports;

public sealed class PersonalDataDtoValidator : AbstractValidator<PersonalDataDto>
{
    public PersonalDataDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.InsuranceId).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Telephone).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(100).EmailAddress();
        RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(20);
        var now = DateTime.UtcNow;
        RuleFor(x => x.DateOfBirth).NotEmpty().InclusiveBetween(
            DateOnly.FromDateTime(now.AddYears(-150)),
            DateOnly.FromDateTime(now)
        );
    }
}