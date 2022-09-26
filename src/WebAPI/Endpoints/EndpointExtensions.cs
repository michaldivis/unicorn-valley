using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using UnicornValley.Domain.Common;

namespace UnicornValley.WebAPI.Endpoints;

public static class EndpointExtensions
{
    public static void HandleError(this IEndpoint endpoint, ILogger logger, IError error)
    {
        AddError(endpoint, error);
        LogError(logger, error);
        throw new ValidationFailureException(endpoint.ValidationFailures, $"{nameof(HandleError)}() called");
    }

    public static void HandleErrors(this IEndpoint endpoint, ILogger logger, IResultBase result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        foreach (var error in result.Errors)
        {
            AddError(endpoint, error);
            LogError(logger, error);
        }

        throw new ValidationFailureException(endpoint.ValidationFailures, $"{nameof(HandleErrors)}() called");
    }

    private static void AddError(IEndpoint endpoint, IError error)
    {
        endpoint.ValidationFailures.Add(new ValidationFailure("GeneralErrors", error.ToString())
        {
            Severity = Severity.Error
        });
    }

    private static void LogError(ILogger logger, IError error)
    {
        //TODO log an IError using ILogger
        logger.LogError(error.ToString());
    }
}
