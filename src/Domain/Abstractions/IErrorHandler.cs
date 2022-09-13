namespace UnicornValley.Domain.Abstractions;
public interface IErrorHandler
{
    Task HandleAsync(IResultBase result, CancellationToken cancellationToken);
}
