using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UnicornValley.WebAPI.Utils;

public static class EndpointUtils
{
    public delegate Task SendAsyncDelegate(ProblemDetails problemDetails, int statusCode, CancellationToken cancellationToken);

    public static async Task SendDomainErrorsAsync(
        IEndpoint endpoint,
        IResultBase result,
        SendAsyncDelegate sendAsyncDelegate,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
        CancellationToken cancellationToken = default)
    {
        var errorResponse = result.Errors.Count > 1
            ? ResponseUtils.CreateMultipleProblemDetails(endpoint, result.Errors, httpStatusCode)
            : ResponseUtils.CreateProblemDetails(endpoint, result.Errors.First(), httpStatusCode);
        await sendAsyncDelegate(errorResponse, (int)httpStatusCode, cancellationToken);
    }

    public static async Task SendDomainErrorsAsync(
        IEndpoint endpoint,
        IError error,
        SendAsyncDelegate sendAsyncDelegate,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
        CancellationToken cancellationToken = default)
    {
        var errorResponse = ResponseUtils.CreateProblemDetails(endpoint, error, httpStatusCode);
        await sendAsyncDelegate(errorResponse, (int)httpStatusCode, cancellationToken);
    }
}
