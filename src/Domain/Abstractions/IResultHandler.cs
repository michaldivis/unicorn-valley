namespace UnicornValley.Domain.Abstractions;
public interface IResultHandler
{
    Task HandleAsync(IResultBase result, CancellationToken cancellationToken = default);
}
