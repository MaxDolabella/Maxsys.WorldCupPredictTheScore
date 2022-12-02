using FluentValidation.Results;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;

public static class ValidationResultExtensions
{
    public static ValidationResult AddError(this ValidationResult validationResult, string errorMessage, string? errorCode = null, string? propertyName = null)
    {
        var failure = new ValidationFailure
        {
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            PropertyName = propertyName ?? string.Empty
        };

        validationResult.Errors.Add(failure);

        return validationResult;
    }
}