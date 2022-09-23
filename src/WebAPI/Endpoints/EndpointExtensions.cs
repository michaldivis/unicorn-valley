using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using UnicornValley.Domain.Common;

namespace UnicornValley.WebAPI.Endpoints;

public static class EndpointExtensions
{
    public static void HandleError(this IEndpoint endpoint, DomainError domainError)
    {
        AddError(endpoint, domainError);
        throw new ValidationFailureException(endpoint.ValidationFailures, $"{nameof(HandleError)}() called");
    }

    public static void HandleErrors(this IEndpoint endpoint, IResultBase result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        foreach (var error in result.Errors)
        {
            AddError(endpoint, error);
        }

        throw new ValidationFailureException(endpoint.ValidationFailures, $"{nameof(HandleErrors)}() called");
    }

    private static void AddError(IEndpoint endpoint, IError error)
    {
        if (error is DomainError domainError)
        {
            endpoint.ValidationFailures.Add(new ValidationFailure("GeneralErrors", domainError.Message)
            {
                ErrorCode = domainError.Code,
                Severity = Severity.Error
            });
            return;
        }

        endpoint.ValidationFailures.Add(new ValidationFailure("GeneralErrors", error.Message)
        {
            Severity = Severity.Error
        });
    }
}