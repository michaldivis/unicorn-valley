using FluentResults;
using UnicornValley.Domain.Common;

namespace UnicornValley.WebAPI.Endpoints;

public static class EndpointUtils
{
    public static void HandleErrorResult<T>(Result<T> result, Action throwIfAnyErrorsDelegate, Action<string, string?, FluentValidation.Severity> addErrorDelegate)
    {
        foreach (var error in result.Errors)
        {
            AddErrorFromResult(error, addErrorDelegate);
        }

        throwIfAnyErrorsDelegate?.Invoke();
    }

    private static void AddErrorFromResult(IError error, Action<string, string?, FluentValidation.Severity> addErrorDelegate)
    {
        if (error is DomainError domainError)
        {
            addErrorDelegate.Invoke(domainError.Message, domainError.Code, FluentValidation.Severity.Error);
            return;
        }

        addErrorDelegate.Invoke(error.Message, null, FluentValidation.Severity.Error);
    }
}
