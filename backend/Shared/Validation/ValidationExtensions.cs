using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Shared.Validation;

public static class ValidationExtensions
{
    public static bool CheckForErrors<T>(
        this IValidator<T> validator,
        T value,
        [NotNullWhen(true)] out IResult? result
    )
    {
        var validationResult = validator.Validate(value);
        if (validationResult.IsValid)
        {
            result = null;
            return false;
        }

        result = TypedResults.ValidationProblem(
            validationResult.ToDictionary(),
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title: "Bad Request"
        );
        return true;
    }
}