using FluentValidation;

namespace Shared.Messages.DamageReports;

public sealed class SubmitDamageReportDtoValidator : AbstractValidator<SubmitDamageReportDto>
{
    public SubmitDamageReportDtoValidator(
        PersonalDataDtoValidator personalDataDtoValidator,
        CircumstancesDtoValidator circumstancesDtoValidator,
        VehicleDamageDtoValidator vehicleDamageDtoValidator
    )
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PersonalData).SetValidator(personalDataDtoValidator);
        RuleFor(x => x.Circumstances).SetValidator(circumstancesDtoValidator);
        RuleFor(x => x.VehicleDamage).SetValidator(vehicleDamageDtoValidator);
    }
}