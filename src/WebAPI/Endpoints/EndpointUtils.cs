using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UnicornValley.Domain.Common;

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
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int)httpStatusCode
        };

        if (error is DomainError domainError)
        {
            problemDetails = new ProblemDetails
            {
                Type = $"/errors/{domainError.Code.Replace('.', '-').ToLower()}",
                Title = domainError.Title,
                Detail = domainError.Message
            };

            problemDetails.Type = $"/errors/{domainError.Code.Replace('.', '-').ToLower()}";
            problemDetails.Title = domainError.Title;
            problemDetails.Detail = string.Format(domainError.Message, domainError.Args);
        }
        else
        {
            problemDetails.Type = "/errors/unknown";
            problemDetails.Detail = error.Message;
        }

        foreach (var item in error.Metadata)
        {
            problemDetails.Extensions.Add(item);
        }

        return problemDetails;
    }
}
