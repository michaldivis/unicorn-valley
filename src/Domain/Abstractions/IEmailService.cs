namespace UnicornValley.Domain.Abstractions;

public interface IEmailService
{
    Task<Result> SendInvitationAcceptedAsync(Meeting meeting, CancellationToken cancellationToken);
}
