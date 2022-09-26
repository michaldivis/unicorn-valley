namespace UnicornValley.Domain.Common;
public record DomainError : IError
{
    public string Code { get; }
    public string Title { get; }
    public string Message { get; }
    public object?[] Args { get; }
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = new();

    internal DomainError(string code, string title, string message, params object?[] args)
    {
        Code = code;
        Title = title;
        Message = message;
        Args = args;
    }

    /// <summary>
    /// Set the metadata
    /// </summary>
    public DomainError WithMetadata(string metadataName, object metadataValue)
    {
        Metadata.Add(metadataName, metadataValue);
        return this;
    }
}