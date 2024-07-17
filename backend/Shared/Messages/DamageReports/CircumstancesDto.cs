using System;
using System.Collections.Generic;

namespace Shared.Messages.DamageReports;

public sealed record CircumstancesDto
{
    private List<PersonDto>? _passengers;
    public DateTime DateOfAccidentUtc { get; init; }
    public AccidentType AccidentType { get; init; }

    public List<PersonDto> Passengers
    {
        get => _passengers ??= [];
        init => _passengers = value;
    }

    public string CountryCode { get; init; } = string.Empty;
    public string ReasonOfTravel { get; init; } = string.Empty;
    public PersonDto? OtherPartyContact { get; init; }
    public string CarType { get; init; } = string.Empty;
    public string CarColor { get; init; } = string.Empty;

    public bool Equals(CircumstancesDto? other)
    {
        if (ReferenceEquals(other, this))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        if (DateOfAccidentUtc != other.DateOfAccidentUtc ||
            AccidentType != other.AccidentType ||
            CountryCode != other.CountryCode ||
            ReasonOfTravel != other.ReasonOfTravel ||
            !EqualityComparer<PersonDto>.Default.Equals(OtherPartyContact, other.OtherPartyContact) ||
            CarType != other.CarType ||
            CarColor != other.CarColor)
        {
            return false;
        }

        return ComparePassengers(_passengers, other._passengers);
    }

    private static bool ComparePassengers(List<PersonDto>? x, List<PersonDto>? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        for (var i = 0; i < x.Count; i++)
        {
            if (!EqualityComparer<PersonDto>.Default.Equals(x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hashCodeBuilder = new HashCode();
        hashCodeBuilder.Add(DateOfAccidentUtc);
        hashCodeBuilder.Add(AccidentType);
        hashCodeBuilder.Add(CountryCode);
        hashCodeBuilder.Add(ReasonOfTravel);
        hashCodeBuilder.Add(OtherPartyContact);
        hashCodeBuilder.Add(CarType);
        hashCodeBuilder.Add(CarColor);
        // ReSharper disable NonReadonlyMemberInGetHashCode
        if (_passengers is not null)
        {
            foreach (var passenger in _passengers)
            {
                hashCodeBuilder.Add(passenger);
            }
        }
        // ReSharper restore NonReadonlyMemberInGetHashCode

        return hashCodeBuilder.ToHashCode();
    }
}