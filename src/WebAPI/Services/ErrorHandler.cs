using FluentResults;
using UnicornValley.Domain.Abstractions;

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
        _logger.LogError("An error occured: {@Error}", error); //TODO log errors better
    }
}
