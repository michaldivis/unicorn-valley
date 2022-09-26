using FluentResults;
using UnicornValley.Domain.Abstractions;
using UnicornValley.Domain.Errors;

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
            _logger.LogError(domainError.Message, domainError.Args);
        }
        else
        {
            _logger.LogError("An unknown error occured: {@Error}", error); //TODO unknown log errors better
        }        
    }
}
