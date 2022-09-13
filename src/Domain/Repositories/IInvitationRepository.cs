namespace UnicornValley.Domain.Repositories;

public interface IInvitationRepository
{
    Task<Result<Invitation>> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Invitation invitation);
}

