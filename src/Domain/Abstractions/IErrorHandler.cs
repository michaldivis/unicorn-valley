namespace UnicornValley.Domain.Abstractions;
public interface IErrorHandler
{
    Task Handle(Result result, bool silent = false);
}
