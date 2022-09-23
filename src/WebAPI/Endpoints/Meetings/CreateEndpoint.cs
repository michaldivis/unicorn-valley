using FastEndpoints;
using FluentResults;
using MediatR;
using UnicornValley.Application.Meetings.Create;
using UnicornValley.Domain.Common;

namespace UnicornValley.WebAPI.Endpoints.Meetings;
public class CreateEndpoint : Endpoint<CreateMeetingCommand>
{
    private readonly IMediator _mediator;

    public CreateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/meetings/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateMeetingCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);
        await HandleResultAsync(result, ct);
    }

    private async Task HandleResultAsync<T>(Result<T> result, CancellationToken ct)
    {
        if (result.IsSuccess)
        {
            await SendAsync(result.Value, cancellation: ct);
            return;
        }

        foreach (var error in result.Errors)
        {
            AddErrorFromResult(error);
        }

        ThrowIfAnyErrors();
    }

    private void AddErrorFromResult(IError error)
    {
        if (error is DomainError domainError)
        {
            AddError(domainError.Message, domainError.Code);
            return;
        }

        AddError(error.Message);
    }
}