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
            Detail = "There were multiple problems that have occurred",
            Instance = endpoint.HttpContext.Request.Path,
            Status = (int)httpStatusCode
        };

        var problems = errors.Select(a => CreateProblemDetails(endpoint, a, null));
        multipleProblemDetails.Extensions.Add("problems", problems);

        return multipleProblemDetails;
    }

    public static ProblemDetails CreateValidationProblemDetails(IEnumerable<FluentValidation.Results.ValidationFailure> failures, int statusCode)
    {
        var validationProblemDetails = new ProblemDetails
        {
            Type = "/errors/multiple-validation-errors",
            Detail = "One or more validation problems occurred",
            Status = statusCode
        };

        var problems = new List<ProblemDetails>();

        foreach (var failure in failures)
        {
            var problem = new ProblemDetails
            {
                Type = "/errors/validation-error",
                Title = "Validation failed",
                Detail = failure.ErrorMessage
            };

            var metadata = new Dictionary<string, object>
                {
                    { "propertyName", failure.PropertyName },
                    { "attemptedValue", failure.AttemptedValue }
                };

            problem.Extensions.Add("metadata", metadata);

            problems.Add(problem);
        }

        validationProblemDetails.Extensions.Add("problems", problems);

        return validationProblemDetails;
    }
}