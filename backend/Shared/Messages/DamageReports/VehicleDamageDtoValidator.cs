using FluentValidation;

namespace Shared.Messages.DamageReports;

public sealed class VehicleDamageDtoValidator : AbstractValidator<VehicleDamageDto>
{
    public VehicleDamageDtoValidator()
    {
        RuleFor(x => x.FrontBumper).IsInEnum();
        RuleFor(x => x.RearBumper).IsInEnum();
        RuleFor(x => x.Hood).IsInEnum();
        RuleFor(x => x.TrunkLid).IsInEnum();
        RuleFor(x => x.Roof).IsInEnum();
        RuleFor(x => x.FrontLeftDoor).IsInEnum();
        RuleFor(x => x.FrontRightDoor).IsInEnum();
        RuleFor(x => x.RearLeftDoor).IsInEnum();
        RuleFor(x => x.RearRightDoor).IsInEnum();
        RuleFor(x => x.FrontLeftFender).IsInEnum();
        RuleFor(x => x.FrontRightFender).IsInEnum();
        RuleFor(x => x.RearLeftFender).IsInEnum();
        RuleFor(x => x.RearRightFender).IsInEnum();
        RuleFor(x => x.Grille).IsInEnum();
        RuleFor(x => x.LeftHeadlights).IsInEnum();
        RuleFor(x => x.RightHeadlights).IsInEnum();
        RuleFor(x => x.LeftTaillights).IsInEnum();
        RuleFor(x => x.RightTaillights).IsInEnum();
        RuleFor(x => x.LeftSideMirror).IsInEnum();
        RuleFor(x => x.RightSideMirror).IsInEnum();
        RuleFor(x => x.Windshield).IsInEnum();
        RuleFor(x => x.RearWindshield).IsInEnum();
        RuleFor(x => x.FrontLeftWindow).IsInEnum();
        RuleFor(x => x.FrontRightWindow).IsInEnum();
        RuleFor(x => x.RearLeftWindow).IsInEnum();
        RuleFor(x => x.RearRightWindow).IsInEnum();
        RuleFor(x => x.FrontLeftWheel).IsInEnum();
        RuleFor(x => x.FrontRightWheel).IsInEnum();
        RuleFor(x => x.RearLeftWheel).IsInEnum();
        RuleFor(x => x.RearRightWheel).IsInEnum();
        RuleFor(x => x.LeftExteriorTrim).IsInEnum();
        RuleFor(x => x.RightExteriorTrim).IsInEnum();
    }
}