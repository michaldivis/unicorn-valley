using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UnicornValley.Domain.Errors;

namespace UnicornValley.WebAPI.Endpoints;

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
            ? CreateMultipleProblemDetails(endpoint, result.Errors, httpStatusCode)
            : CreateProblemDetails(endpoint, result.Errors.First(), httpStatusCode);
        await sendAsyncDelegate(errorResponse, (int)httpStatusCode, cancellationToken);
    }

    public static async Task SendDomainErrorsAsync(
        IEndpoint endpoint,
        IError error,
        SendAsyncDelegate sendAsyncDelegate,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
        CancellationToken cancellationToken = default)
    {
        var errorResponse = CreateProblemDetails(endpoint, error, httpStatusCode);
        await sendAsyncDelegate(errorResponse, (int)httpStatusCode, cancellationToken);
    }

    private static ProblemDetails CreateProblemDetails(IEndpoint endpoint, IError error, HttpStatusCode? httpStatusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = error.Message,
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int?)httpStatusCode
        };

        if (error is DomainError domainError)
        {
            problemDetails.Type = $"/errors/{domainError.Code}";
            problemDetails.Title = domainError.Title;
        }
        else
        {
            problemDetails.Type = "/errors/unknown";
        }

        if (error.Metadata.Any())
        {
            problemDetails.Extensions.Add("metadata", error.Metadata);
        }

        return problemDetails;
    }

    private static ProblemDetails CreateMultipleProblemDetails(IEndpoint endpoint, IEnumerable<IError> errors, HttpStatusCode httpStatusCode)
    {
        var multipleProblemDetails = new ProblemDetails
        {
            Type = "/errors/multiple",
            Detail = "There were multiple problems that have occurred",
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int)httpStatusCode
        };

        var problems = errors.Select(a => CreateProblemDetails(endpoint, a, null));
        multipleProblemDetails.Extensions.Add("problems", problems);

        return multipleProblemDetails;
    }
}
