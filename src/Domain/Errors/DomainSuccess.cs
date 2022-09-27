namespace UnicornValley.Domain.Errors;

public class DomainSuccess : ISuccess
{
    public string Code { get; }
    public string Title { get; }
    public string Message { get; }
    public string MessageTemplate { get; }
    public object?[] Args { get; }
    public Dictionary<string, object> Metadata { get; } = new();
    public List<IError> Reasons { get; } = new();

    public DomainSuccess(string code, string title, string messageTemplate, object?[] args, Dictionary<string, object> metadata)
    {
        Code = code;
        Title = title;
        MessageTemplate = messageTemplate;
        Args = args;
        Metadata = metadata;

        Message = string.Format(messageTemplate, args);
    }
}
