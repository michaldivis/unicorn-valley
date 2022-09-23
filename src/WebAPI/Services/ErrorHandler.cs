using FluentResults;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Common;

namespace UnicornValley.WebAPI.Services;

public class ErrorHandler : IErrorHandler
{
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(ILogger<ErrorHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(IResultBase result, CancellationToken cancellationToken)
    {
        if (result.IsSuccess)
        {
            return Task.CompletedTask;
        }

        foreach (var error in result.Errors)
        {
            LogError(error);
        }

        return Task.CompletedTask;
    }

    private void LogError(IError error)
    {
        if(error is DomainError domainError)
        {
            _logger.LogWarning("Domain error occured with code: {@Code} and message: {@Message}", domainError.Code, domainError.Message);
            return;
        }

        _logger.LogError("An error occured: {@Error}", error); //TODO log unknown errors better
    }
}
