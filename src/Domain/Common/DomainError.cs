namespace UnicornValley.Domain.Common;
public class DomainError : IError
{
    public string Code { get; }
    public string Title { get; }
    public string Message { get; }
    public Dictionary<string, object> Metadata { get; }
    public List<IError> Reasons { get; }

    public DomainError(string code, string title, string message)
    {
        Code = code;
        Title = title;
        Message = message;
        Metadata = new Dictionary<string, object>();
        Reasons = new List<IError>();
    }

    /// <summary>
    /// Set the metadata
    /// </summary>
    public DomainError WithMetadata(string metadataName, object metadataValue)
    {
        Metadata.Add(metadataName, metadataValue);
        return this;
    }

    public override string ToString()
    {
        return new ReasonStringBuilder()
            .WithReasonType(GetType())
            .WithInfo(nameof(Code), Code)
            .WithInfo(nameof(Title), Title)
            .WithInfo(nameof(Message), Message)
            .WithInfo(nameof(Metadata), string.Join("; ", Metadata))
            .WithInfo(nameof(Reasons), string.Join("; ", Reasons))
            .Build();
    }
}