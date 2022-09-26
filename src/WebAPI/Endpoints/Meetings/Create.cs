using UnicornValley.Application.Meetings.Create;

namespace UnicornValley.WebAPI.Endpoints.Meetings;
public class Create : Endpoint<CreateMeetingCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogger<Create> _logger;

    public Create(IMediator mediator, ILogger<Create> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/meetings/create");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a meeting";
            s.ExampleRequest = new CreateMeetingCommand
            {
                CreatorId = Guid.NewGuid(),
                Name = "Workshop: Unicorns an where to find them",
                Location = "Hierarch Square, Novigrad",
                Type = Domain.Enums.MeetingType.WithLimitedNumberOfAttendees,
                MaximumNumberOfAttendees = 150,
                ScheduledAtUtc = DateTime.UtcNow.AddMonths(1)
            };
        });
    }

    public override async Task HandleAsync(CreateMeetingCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);

        if (result.IsSuccess)
        {
            await SendAsync(result.Value, cancellation: ct);
            return;
        }

        this.HandleErrors(_logger, result);
    }
}