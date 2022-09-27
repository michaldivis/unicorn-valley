using FluentResults;
using UnicornValley.Domain.Abstractions;

namespace UnicornValley.WebAPI.Services;

public class ResultHandler : IResultHandler
{
    private readonly ILogger<ResultHandler> _logger;

    public ResultHandler(ILogger<ResultHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(IResultBase result, CancellationToken cancellationToken)
    {
        foreach (var success in result.Successes)
        {
            _logger.LogInformation("Success: {@Success}", success);
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError("Error: {@Error}", error);
        }

        return Task.CompletedTask;
    }
}
