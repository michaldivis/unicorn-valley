using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UnicornValley.Domain.Errors;

namespace UnicornValley.WebAPI.Utils;

public static class ResponseUtils
{
    public static ProblemDetails CreateProblemDetails(IEndpoint endpoint, IError error, HttpStatusCode? httpStatusCode)
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

    public static ProblemDetails CreateMultipleProblemDetails(IEndpoint endpoint, IEnumerable<IError> errors, HttpStatusCode httpStatusCode)
    {
        var multipleProblemDetails = new ProblemDetails
        {
            Type = "/errors/multiple",
            Title = "Multiple problems occured",
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int)httpStatusCode
        };

        var problems = errors.Select(a => CreateProblemDetails(endpoint, a, null));
        multipleProblemDetails.Extensions.Add("problems", problems);

        return multipleProblemDetails;
    }

    public static ProblemDetails CreateValidationProblemDetails(IEnumerable<FluentValidation.Results.ValidationFailure> failures, HttpContext? context, int statusCode)
    {
        var validationProblemDetails = new ProblemDetails
        {
            Type = "/errors/validation",
            Title = "One or more validation errors occurred",
            Instance = context?.Request.Path,
            Status = statusCode
        };

        var errors = failures.Select(a => new
        {
            Property = a.PropertyName,
            Message = a.ErrorMessage,
            Value = a.AttemptedValue
        });

        validationProblemDetails.Extensions.Add("errors", errors);

        return validationProblemDetails;
    }
}