namespace UnicornValley.Domain.Repositories;

public interface IMeetingRepository
{
    Task<Result<Meeting>> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Meeting meeting);
}