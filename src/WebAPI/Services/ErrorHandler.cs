﻿using FluentResults;
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

        //TODO turn error result into log message
        _logger.LogError("Error occured {@Error}", result.ToString());

        return Task.CompletedTask;
    }
}
