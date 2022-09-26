using FluentResults;
using FluentValidation.Internal;
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
        //TODO handle mulitple errors
        var errorResponse = CreateProblemDetails(endpoint, result.Errors.First(), httpStatusCode);
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

    private static ProblemDetails CreateProblemDetails(IEndpoint endpoint, IError error, HttpStatusCode httpStatusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = error.Message,
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int)httpStatusCode
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

        foreach (var item in error.Metadata)
        {
            problemDetails.Extensions.Add(item);
        }

        return problemDetails;
    }
}
